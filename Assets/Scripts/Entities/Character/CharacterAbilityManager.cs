using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : MonoBehaviour
{
    private Character character;

    public Ability[] CharacterAbilities { get; private set; }
    public Ability[] PassiveCharacterAbilities { get; private set; }
    public Ability[] OtherCharacterAbilities { get; private set; }
    public Ability[] SummonerAbilities { get; private set; }
    public Ability[] OfflineAbilities { get; private set; }

    private List<Ability> currentlyUsedAbilities;
    private List<Ability> characterAbilitiesWithResourceCosts;

    public delegate void OnAnAbilityUsedHandler();
    public event OnAnAbilityUsedHandler OnAnAbilityUsed;

    private CharacterAbilityManager()
    {
        currentlyUsedAbilities = new List<Ability>();
        characterAbilitiesWithResourceCosts = new List<Ability>();
    }

    private void Start()
    {
        character = GetComponent<Character>();
        InitAbilities();

        if (character.AbilityUIManager)
        {
            character.EntityStats.Resource.OnCurrentResourceValueChanged += OnResourceCurrentValueChanged;
        }
    }

    private void InitAbilities()
    {
        bool isLocalCharacter = character.AbilityUIManager;

        CharacterAbility[] iCharacterAbilities = GetComponents<CharacterAbility>();
        CharacterAbilities = new Ability[iCharacterAbilities.Length];
        for (int i = 0; i < CharacterAbilities.Length; i++)
        {
            Ability ability = (Ability)iCharacterAbilities[i];
            CharacterAbilities[i] = ability;
            ability.OnAbilityUsed += OnAbilityUsed;
            ability.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability.ID = i;
                ability.AbilityCategory = AbilityCategory.CharacterAbility;
                ability.SetAbilitySprite();
                characterAbilitiesWithResourceCosts.Add(ability);
                character.AbilityUIManager.DisableAbility(AbilityCategory.CharacterAbility, i, false);
                character.AbilityUIManager.SetMaxAbilityLevel(i, ability.MaxLevel);
            }
        }

        PassiveCharacterAbility[] iPassiveCharacterAbilities = GetComponents<PassiveCharacterAbility>();
        PassiveCharacterAbilities = new Ability[iPassiveCharacterAbilities.Length];
        for (int j = 0; j < PassiveCharacterAbilities.Length; j++)
        {
            Ability ability2 = (Ability)iPassiveCharacterAbilities[j];
            PassiveCharacterAbilities[j] = ability2;
            ability2.OnAbilityUsed += OnAbilityUsed;
            ability2.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability2.ID = j;
                ability2.AbilityCategory = AbilityCategory.PassiveCharacterAbility;
                ability2.SetAbilitySprite();
            }
        }

        OtherCharacterAbility[] iOtherCharacterAbilities = GetComponents<OtherCharacterAbility>();
        OtherCharacterAbilities = new Ability[iOtherCharacterAbilities.Length];
        for (int k = 0; k < OtherCharacterAbilities.Length; k++)
        {
            Ability ability3 = (Ability)iOtherCharacterAbilities[k];
            OtherCharacterAbilities[k] = ability3;
            ability3.OnAbilityUsed += OnAbilityUsed;
            ability3.OnAbilityFinished += OnAbilityFinished;
        }

        SummonerAbility[] iSummonerAbilities = GetComponents<SummonerAbility>();
        SummonerAbilities = new Ability[iSummonerAbilities.Length];
        for (int l = 0; l < SummonerAbilities.Length; l++)
        {
            Ability ability4 = (Ability)iSummonerAbilities[l];
            SummonerAbilities[l] = ability4;
            ability4.OnAbilityUsed += OnAbilityUsed;
            ability4.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability4.ID = l;
                ability4.AbilityCategory = AbilityCategory.SummonerAbility;
                ability4.SetAbilitySprite();
            }
        }

        if (!StaticObjects.OnlineMode)
        {
            OfflineAbility[] iOfflineAbilities = GetComponents<OfflineAbility>();
            OfflineAbilities = new Ability[iOfflineAbilities.Length];
            for (int m = 0; m < OfflineAbilities.Length; m++)
            {
                Ability ability5 = (Ability)iOfflineAbilities[m];
                OfflineAbilities[m] = ability5;
                ability5.OnAbilityUsed += OnAbilityUsed;
                ability5.OnAbilityFinished += OnAbilityFinished;
            }
        }
    }

    private void OnResourceCurrentValueChanged()
    {
        float currentValue = character.EntityStats.Resource.GetCurrentValue();
        for (int i = characterAbilitiesWithResourceCosts.Count - 1; i >= 0; i--)
        {
            UpdateAbilityHasEnoughResource(characterAbilitiesWithResourceCosts[i], currentValue);
        }
    }

    private void UpdateAbilityHasEnoughResource(Ability ability, float currentValue)
    {
        if (ability.UsesResource)
        {
            bool hasEnoughResourceToCastAbility = !ability.IsEnabled || ability.IsOnCooldown || currentValue >= ability.GetResourceCost();
            character.AbilityUIManager.UpdateAbilityHasEnoughResource(ability.ID, hasEnoughResourceToCastAbility);
        }
        else
        {
            character.AbilityUIManager.UpdateAbilityHasEnoughResource(ability.ID, true);
            characterAbilitiesWithResourceCosts.Remove(ability);
        }
    }

    private Ability GetAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = null;
        switch (abilityCategory)
        {
            case AbilityCategory.CharacterAbility:
                ability = CharacterAbilities[abilityId];
                break;
            case AbilityCategory.PassiveCharacterAbility:
                ability = PassiveCharacterAbilities[abilityId];
                break;
            case AbilityCategory.OtherCharacterAbility:
                ability = OtherCharacterAbilities[abilityId];
                break;
            case AbilityCategory.SummonerAbility:
                ability = SummonerAbilities[abilityId];
                break;
            case AbilityCategory.OfflineAbility:
                ability = OfflineAbilities[abilityId];
                break;
        }
        return ability;
    }

    private void SendToServer_Ability_Destination(AbilityCategory abilityCategory, int abilityId, Vector3 destination)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Destination", PhotonTargets.AllViaServer, abilityCategory, abilityId, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Destination(AbilityCategory abilityCategory, int abilityId, Vector3 destination)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsCastable(ability))
        {
            UsePositionTargetedAbility(ability, destination);
        }
    }

    private void SendToServer_Ability_Entity(AbilityCategory abilityCategory, int abilityId, Entity target)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Entity", PhotonTargets.AllViaServer, abilityCategory, abilityId, target.EntityId, target.EntityType);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Entity(AbilityCategory abilityCategory, int abilityId, int entityId, EntityType entityType)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsCastable(ability))
        {
            UseUnitTargetedAbility(ability, FindTarget(entityId, entityType));
        }
    }

    private void SendToServer_Ability_Recast(AbilityCategory abilityCategory, int abilityId)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Recast", PhotonTargets.AllViaServer, abilityCategory, abilityId);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Recast(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
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

        Ability bufferedAbility = character.CharacterBufferedAbilityManager.GetBufferedAbility();
        if (bufferedAbility != null)
        {
            if (!bufferedAbility.UsesResource || bufferedAbility.GetResourceCost() <= character.EntityStats.Resource.GetCurrentValue())
            {
                character.CharacterMovement.StopAllMovement(false);
                character.CharacterBufferedAbilityManager.UseBufferedAbility();
            }
            else
            {
                character.CharacterBufferedAbilityManager.ResetBufferedAbility();
            }
        }
    }

    public void LevelUpAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        ability.LevelUp();
        ability.UpdateLevelOnUI();
    }

    public void OnPressedInputForAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (ability.IsEnabled)
        {
            if (AbilityIsCastable(ability))
            {
                if (ability is UnitTargeted)
                {
                    Entity hoveredEntity = character.CharacterMouseManager.HoveredEntity;
                    if (hoveredEntity != null && ability.CanBeCast(character.CharacterMouseManager.HoveredEntity))
                    {
                        if (StaticObjects.OnlineMode)
                        {
                            SendToServer_Ability_Entity(abilityCategory, abilityId, hoveredEntity);
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
                        SendToServer_Ability_Destination(abilityCategory, abilityId, ability.GetDestination());
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
                    SendToServer_Ability_Recast(abilityCategory, abilityId);
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
                character.CharacterBufferedAbilityManager.ResetBufferedAbility();
            }
            ability.UseAbility(destination);
            if (ability.HasCastTime || ability.HasChannelTime || ability.CanBeRecasted)
            {
                character.CharacterMovement.SetCharacterIsInRangeEventForBasicAttack();
            }
        }
        else
        {
            //character.CharacterMovement.StopAllMovement();
            character.CharacterBufferedAbilityManager.BufferPositionTargetedAbility(ability, destination);
        }
    }

    private void UseUnitTargetedAbility(Ability ability, Entity target)
    {
        if (!IsUsingAbilityPreventingAbilityCast(ability))
        {
            if (currentlyUsedAbilities.Count > 0)
            {
                character.CharacterBufferedAbilityManager.ResetBufferedAbility();
            }
            character.CharacterMovement.StopAllMovement();
            ability.UseAbility(target);
        }
        else
        {
            //character.CharacterMovement.StopAllMovement();
            character.CharacterBufferedAbilityManager.BufferUnitTargetedAbility(ability, target);
        }
    }

    //True: Allow ability to be cast
    //False: Act as if the key was not pressed
    private bool AbilityIsCastable(Ability abilityToCast)
    {
        if (abilityToCast.IsOnCooldown || abilityToCast.IsOnCooldownForRecast || abilityToCast.IsActive)
        {
            return false;
        }

        if (abilityToCast.UsesResource && abilityToCast.GetResourceCost() > character.EntityStats.Resource.GetCurrentValue())
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.CannotCastAnyAbilityWhileActive)
            {
                return false;
            }
        }

        return true;
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
            if (CannotCastAbility(abilityToCast, ability))
            {
                return true;
            }
        }

        return false;
    }

    private bool CannotCastAbility(Ability abilityToCast, Ability ability)
    {
        return ability.HasCastTime && ability.IsBeingCasted &&
            (ability.CastableAbilitiesWhileActive == null || !CanCastAbilityWhileActive(ability.CastableAbilitiesWhileActive, abilityToCast)) &&
            !(ability.HasChannelTime && ability.IsBeingChanneled && ability.CanUseAnyAbilityWhileChanneling);
    }

    private bool CanCastAbilityWhileActive(Ability[] castableAbilitiesWhileActive, Ability abilityToCast)
    {
        bool canCast = false;

        foreach (Ability ability in castableAbilitiesWhileActive)
        {
            if (ability == abilityToCast)
            {
                canCast = true;
                break;
            }
        }

        return canCast;
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
        for (int i = currentlyUsedAbilities.Count - 1; i >= 0; i--)
        {
            Ability ability = currentlyUsedAbilities[i];
            if (ability is DirectionTargetedDash)
            {
                ability.CancelAbility();
            }
        }
    }

    public void ResetCooldowns()
    {
        for (int i = 0; i < CharacterAbilities.Length; i++)
        {
            GetAbility(AbilityCategory.CharacterAbility, i).ResetCooldown();
        }
        for (int j = 0; j < PassiveCharacterAbilities.Length; j++)
        {
            GetAbility(AbilityCategory.PassiveCharacterAbility, j).ResetCooldown();
        }
        for (int k = 0; k < OtherCharacterAbilities.Length; k++)
        {
            GetAbility(AbilityCategory.OtherCharacterAbility, k).ResetCooldown();
        }
        for (int l = 0; l < SummonerAbilities.Length; l++)
        {
            GetAbility(AbilityCategory.SummonerAbility, l).ResetCooldown();
        }
        if (!StaticObjects.OnlineMode)
        {
            for (int m = 0; m < OfflineAbilities.Length; m++)
            {
                GetAbility(AbilityCategory.OfflineAbility, m).ResetCooldown();
            }
        }
    }

    public int[] GetCharacterAbilityLevels()
    {
        return new int[] { CharacterAbilities[0].AbilityLevel, CharacterAbilities[1].AbilityLevel, CharacterAbilities[2].AbilityLevel, CharacterAbilities[3].AbilityLevel };
    }

    public void SetAbilityLevelsFromLoad(int[] characterAbilityLevels)//TODO: Check all this works out
    {
        for (int i = 0; i < CharacterAbilities.Length; i++)
        {
            CharacterAbilities[0].LevelUp();
        }
        for (int j = 0; j < characterAbilityLevels[1]; j++)
        {
            CharacterAbilities[1].LevelUp();
        }
        for (int k = 0; k < characterAbilityLevels[2]; k++)
        {
            CharacterAbilities[2].LevelUp();
        }
        for (int l = 0; l < characterAbilityLevels[3]; l++)
        {
            CharacterAbilities[3].LevelUp();
        }
    }
}
