using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : MonoBehaviour
{
    private Character character;

    private Dictionary<AbilityInput, Ability> abilities;
    private List<Ability> currentlyUsedAbilities;

    public delegate void OnAnAbilityUsedHandler();
    public event OnAnAbilityUsedHandler OnAnAbilityUsed;

    private CharacterAbilityManager()
    {
        abilities = new Dictionary<AbilityInput, Ability>();
        currentlyUsedAbilities = new List<Ability>();
    }

    private void Start()
    {
        character = GetComponent<Character>();
        InitAbilities();
    }

    private void InitAbilities()
    {
        Ability[] abilitiesOnCharacter = GetComponents<Ability>();
        for (int i = 0; i < abilitiesOnCharacter.Length; i++)
        {
            Ability ability = abilitiesOnCharacter[i];
            ability.OnAbilityUsed += OnAbilityUsed;
            ability.OnAbilityFinished += OnAbilityFinished;
            if (!(ability is OtherAbility) && (!StaticObjects.OnlineMode || character.PhotonView.isMine))
            {
                ability.ID = i;
                character.AbilityUIManager.SetAbilitySprite(i, ability.abilitySprite);
            }

            abilities.Add((AbilityInput)i, ability);
        }
    }

    private void SendToServer_Ability_Destination(AbilityInput abilityInput, Vector3 destination)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Destination", PhotonTargets.AllViaServer, abilityInput, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Destination(AbilityInput abilityInput, Vector3 destination)
    {
        Ability ability = abilities[abilityInput];
        if (AbilityIsCastable(ability))
        {
            UsePositionTargetedAbility(ability, destination);
        }
    }

    private void SendToServer_Ability_Entity(AbilityInput abilityInput, Entity target)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Entity", PhotonTargets.AllViaServer, abilityInput, target.EntityId, target.EntityType);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Entity(AbilityInput abilityInput, int entityId, EntityType entityType)
    {
        Ability ability = abilities[abilityInput];
        if (AbilityIsCastable(ability))
        {
            UseUnitTargetedAbility(ability, FindTarget(entityId, entityType));
        }
    }

    private void SendToServer_Ability_Recast(AbilityInput abilityInput)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Recast", PhotonTargets.AllViaServer, abilityInput);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Recast(AbilityInput abilityInput)
    {
        Ability ability = abilities[abilityInput];
        if (ability.CanBeRecasted && currentlyUsedAbilities.Contains(ability))
        {
            ability.RecastAbility();
        }
    }

    public Entity FindTarget(int entityId, EntityType entityType) // TODO: when adding an EntityType
    {
        Entity entity = null;
        switch (entityType)
        {
            case EntityType.CHARACTER:
                foreach (Character character in FindObjectsOfType<Character>())
                {
                    if (character.EntityId == entityId)
                    {
                        entity = character;
                        break;
                    }
                }
                break;
                /*case EntityType.MINION:
                    foreach (Minion minion in FindObjectsOfType<Minion>())
                    {
                        if (minion.EntityId == entityId)
                        {
                            entity = minion;
                            break;
                        }
                    }
                    break;*/
        }
        return entity;
    }

    private void OnAbilityUsed(Ability ability)
    {
        currentlyUsedAbilities.Add(ability);
        if (OnAnAbilityUsed != null)
        {
            OnAnAbilityUsed();
        }
    }

    private void OnAbilityFinished(Ability ability)
    {
        currentlyUsedAbilities.Remove(ability);

        character.CharacterMovement.RotateCharacterIfMoving();

        character.CharacterActionManager.UseBufferedAction();
    }

    public void OnPressedInputForAbility(AbilityInput abilityInput)
    {
        if (abilities.ContainsKey(abilityInput))
        {
            Ability ability = abilities[abilityInput];
            if (AbilityIsCastable(ability))
            {
                if (ability is UnitTargeted)
                {
                    Entity hoveredEntity = character.CharacterMouseManager.HoveredEntity;
                    if (hoveredEntity != null && ability.CanBeCast(character.CharacterMouseManager.HoveredEntity))
                    {
                        if (StaticObjects.OnlineMode)
                        {
                            SendToServer_Ability_Entity(abilityInput, hoveredEntity);
                        }
                        else
                        {
                            UseUnitTargetedAbility(ability, hoveredEntity);
                        }
                    }
                }
                else if (ability.CanBeCast(Input.mousePosition))
                {
                    if (StaticObjects.OnlineMode)
                    {
                        SendToServer_Ability_Destination(abilityInput, ability.GetDestination());
                    }
                    else
                    {
                        UsePositionTargetedAbility(ability, ability.GetDestination());
                    }
                }
            }
            else if (ability.CanBeRecasted && !ability.IsOnCooldownForRecast && currentlyUsedAbilities.Contains(ability))
            {
                if (StaticObjects.OnlineMode)
                {
                    SendToServer_Ability_Recast(abilityInput);
                }
                else
                {
                    ability.RecastAbility();
                }
            }
        }
    }

    private void UsePositionTargetedAbility(Ability ability, Vector3 destination)
    {
        if (!IsUsingAbilityPreventingAbilityCast(ability))
        {
            if (currentlyUsedAbilities.Count > 0)
            {
                character.CharacterActionManager.ResetBufferedAction();
            }
            ability.UseAbility(destination);
            if (ability.HasCastTime || ability.HasChannelTime || ability.CanBeRecasted)
            {
                character.CharacterMovement.SetCharacterIsInRangeEventForBasicAttack();
            }
        }
        else
        {
            character.CharacterMovement.StopAllMovement();
            character.CharacterActionManager.SetPositionTargetedAbilityInQueue(ability, destination);
        }
    }

    private void UseUnitTargetedAbility(Ability ability, Entity target)
    {
        if (!IsUsingAbilityPreventingAbilityCast(ability))
        {
            if (currentlyUsedAbilities.Count > 0)
            {
                character.CharacterActionManager.ResetBufferedAction();
            }
            character.CharacterMovement.StopAllMovement();
            ability.UseAbility(target);
        }
        else
        {
            character.CharacterMovement.StopAllMovement();
            character.CharacterActionManager.SetUnitTargetedAbilityInQueue(ability, target);
        }
    }

    //True: Put ability in the buffer
    //False: Allow ability to be cast
    private bool IsUsingAbilityPreventingAbilityCast(Ability abilityToCast)
    {
        if (currentlyUsedAbilities.Count == 0 || abilityToCast.CanBeCastDuringOtherAbilityCastTimes)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!(ability.HasChannelTime && ability.IsBeingChanneled && ability.CanUseAnyAbilityWhileChanneling) && 
                ((ability.HasCastTime || ability.HasChannelTime) && (!ability.CanCastSomeAbilitiesWhileActive || ability.CastableAbilitiesWhileActive.Contains(abilityToCast))))
            {
                return true;
            }
        }

        return false;
    }

    //True: Allow ability to be cast
    //False: Act as if the key was not pressed
    private bool AbilityIsCastable(Ability abilityToCast)
    {
        bool abilityToCastIsAvailable = abilityToCast != null;

        if (abilityToCastIsAvailable && (abilityToCast.IsOnCooldown || abilityToCast.IsOnCooldownForRecast || abilityToCast.IsBeingCasted))
        {
            return false;
        }

        if (currentlyUsedAbilities.Count == 0 || (abilityToCastIsAvailable && abilityToCast.CanBeCastDuringOtherAbilityCastTimes))
        {
            foreach (Ability ability in currentlyUsedAbilities)
            {
                if (ability.CannotCastAnyAbilityWhileActive)// || ability.UncastableAbilitiesWhileActive.Contains(abilityToCast))
                {
                    return false;
                }
            }
            return true;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.CannotCastAnyAbilityWhileActive || (ability.CanCastSomeAbilitiesWhileActive && !ability.CastableAbilitiesWhileActive.Contains(abilityToCast)))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsUsingAbilityPreventingMovement()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!CanMoveWhileAbilityIsActive(ability))
            {
                return true;
            }
        }

        return false;
    }

    private bool CanMoveWhileAbilityIsActive(Ability ability)
    {
        return ability.CanMoveWhileActive || (ability.CanMoveWhileChanneling && ability.IsBeingChanneled);
    }

    public bool IsUsingAbilityPreventingBasicAttacks()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!ability.CanUseBasicAttacksWhileCasting)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsUsingAbilityPreventingRotation()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.CannotRotateWhileCasting)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsUsingADashAbility()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.GetAbilityType() == AbilityType.Dash)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsUsingAbilityThatHasACastTime()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.HasCastTime)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsUsingAbilityThatHasAChannelTime()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.HasChannelTime)
            {
                return true;
            }
        }

        return false;
    }

    public void StopAllDashAbilities()
    {
        for (int i = 0; i < currentlyUsedAbilities.Count; i++)
        {
            Ability ability = currentlyUsedAbilities[i];
            if (ability is DirectionTargetedDash)
            {
                ((DirectionTargetedDash)ability).StopDash();
                i--;
            }
        }
    }

    public void ResetCooldowns()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[(AbilityInput)i].ResetCooldown();
        }
    }
}
