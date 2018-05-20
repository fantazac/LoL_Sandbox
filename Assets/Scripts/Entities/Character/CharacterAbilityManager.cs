using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : MonoBehaviour
{
    protected Character character;

    public Ability[] CharacterAbilities { get; protected set; }
    public Ability[] PassiveCharacterAbilities { get; protected set; }
    public Ability[] OtherCharacterAbilities { get; protected set; }
    public Ability[] SummonerAbilities { get; protected set; }
    public Ability[] OfflineAbilities { get; protected set; }

    protected List<Ability> currentlyUsedAbilities;
    protected List<Ability> characterAbilitiesWithResourceCosts;

    public delegate void OnAnAbilityUsedHandler();
    public event OnAnAbilityUsedHandler OnAnAbilityUsed;

    protected CharacterAbilityManager()
    {
        currentlyUsedAbilities = new List<Ability>();
        characterAbilitiesWithResourceCosts = new List<Ability>();
    }

    protected void Start()
    {
        character = GetComponent<Character>();
        InitAbilities();

        if (character.AbilityUIManager)
        {
            character.EntityStats.Resource.OnCurrentResourceValueChanged += OnResourceCurrentValueChanged;
        }
    }

    protected virtual void InitAbilities()
    {
        OtherCharacterAbilities = new Ability[] { gameObject.AddComponent<Recall>() };
        if (!StaticObjects.OnlineMode)
        {
            OfflineAbilities = new Ability[] { gameObject.AddComponent<DestroyAllDummies>(), gameObject.AddComponent<SpawnAllyDummy>(), gameObject.AddComponent<SpawnEnemyDummy>() };
        }

        bool isLocalCharacter = character.AbilityUIManager;

        for (int i = 0; i < CharacterAbilities.Length; i++)
        {
            Ability ability = CharacterAbilities[i];
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

        for (int j = 0; j < PassiveCharacterAbilities.Length; j++)
        {
            Ability ability2 = PassiveCharacterAbilities[j];
            ability2.OnAbilityUsed += OnAbilityUsed;
            ability2.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability2.ID = j;
                ability2.AbilityCategory = AbilityCategory.PassiveCharacterAbility;
                ability2.SetAbilitySprite();
            }
        }

        for (int k = 0; k < OtherCharacterAbilities.Length; k++)
        {
            Ability ability3 = OtherCharacterAbilities[k];
            ability3.OnAbilityUsed += OnAbilityUsed;
            ability3.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability3.ID = k;
                ability3.AbilityCategory = AbilityCategory.OtherCharacterAbility;
                ability3.SetAbilitySprite();
            }
        }

        for (int l = 0; l < SummonerAbilities.Length; l++)
        {
            Ability ability4 = SummonerAbilities[l];
            ability4.OnAbilityUsed += OnAbilityUsed;
            ability4.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability4.ID = l;
                ability4.AbilityCategory = AbilityCategory.SummonerAbility;
                ability4.SetAbilitySprite();
            }
        }

        if (OfflineAbilities != null)
        {
            for (int m = 0; m < OfflineAbilities.Length; m++)
            {
                Ability ability5 = OfflineAbilities[m];
                ability5.OnAbilityUsed += OnAbilityUsed;
                ability5.OnAbilityFinished += OnAbilityFinished;
                ability5.ID = m;
                ability5.AbilityCategory = AbilityCategory.OfflineAbility;
            }
        }
    }

    protected void OnResourceCurrentValueChanged()
    {
        float currentValue = character.EntityStats.Resource.GetCurrentValue();
        for (int i = characterAbilitiesWithResourceCosts.Count - 1; i >= 0; i--)
        {
            UpdateAbilityHasEnoughResource(characterAbilitiesWithResourceCosts[i], currentValue);
        }
    }

    protected void UpdateAbilityHasEnoughResource(Ability ability, float currentValue)
    {
        if (ability.UsesResource)
        {
            if (!(ability.IsActive || ability.IsOnCooldown || ability.IsOnCooldownForRecast))
            {
                bool hasEnoughResourceToCastAbility = !ability.IsEnabled || ability.IsBlocked || ability.IsOnCooldown || currentValue >= ability.GetResourceCost();
                character.AbilityUIManager.UpdateAbilityHasEnoughResource(ability.ID, hasEnoughResourceToCastAbility);
            }
        }
        else
        {
            character.AbilityUIManager.UpdateAbilityHasEnoughResource(ability.ID, true);
            characterAbilitiesWithResourceCosts.Remove(ability);
        }
    }

    public void BlockAllBasicAbilities()
    {
        foreach (Ability ability in CharacterAbilities)
        {
            ability.BlockAbility();
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.BlockAbility();
            }
        }
        foreach (Ability ability in OtherCharacterAbilities)
        {
            ability.BlockAbility();
        }
    }

    public void UnblockAllBasicAbilities(bool cannotUseLongRangedAbilities, bool cannotUseMovementAbilities, bool cannotUseSummonerAbilities)
    {
        foreach (Ability ability in CharacterAbilities)
        {
            if (!(ability.IsLongRanged && cannotUseLongRangedAbilities) && !(ability.IsAMovementAbility && cannotUseMovementAbilities))
            {
                ability.UnblockAbility();
            }
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (!cannotUseSummonerAbilities && !(ability.IsLongRanged && cannotUseLongRangedAbilities) && !(ability.IsAMovementAbility && cannotUseMovementAbilities))
            {
                ability.UnblockAbility();
            }
        }
        foreach (Ability ability in OtherCharacterAbilities)
        {
            ability.UnblockAbility();
        }
    }

    public void BlockAllLongRangedAbilities()
    {
        foreach (Ability ability in CharacterAbilities)
        {
            if (ability.IsLongRanged)
            {
                ability.BlockAbility();
            }
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (ability.IsLongRanged)
            {
                ability.BlockAbility();
            }
        }
    }

    public void UnblockAllLongRangedAbilities()
    {
        foreach (Ability ability in CharacterAbilities)
        {
            if (ability.IsLongRanged)
            {
                ability.UnblockAbility();
            }
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (ability.IsLongRanged)
            {
                ability.UnblockAbility();
            }
        }
    }

    public void BlockAllMovementAbilities()
    {
        foreach (Ability ability in CharacterAbilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.BlockAbility();
            }
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.BlockAbility();
            }
        }
    }

    public void UnblockAllMovementAbilities()
    {
        foreach (Ability ability in CharacterAbilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.UnblockAbility();
            }
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.UnblockAbility();
            }
        }
    }

    public void BlockAllSummonerAbilities()
    {
        foreach (Ability ability in SummonerAbilities)
        {
            ability.BlockAbility();
        }
    }

    public void UnblockAllSummonerAbilities(bool cannotUseBasicAbilities)
    {
        foreach (Ability ability in SummonerAbilities)
        {
            if (!(ability.IsAMovementAbility && cannotUseBasicAbilities))
            {
                ability.UnblockAbility();
            }
        }
    }

    protected Ability GetAbility(AbilityCategory abilityCategory, int abilityId)
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

    protected void SendToServer_Ability_Destination(AbilityCategory abilityCategory, int abilityId, Vector3 destination)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Destination", PhotonTargets.AllViaServer, abilityCategory, abilityId, destination);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_Destination(AbilityCategory abilityCategory, int abilityId, Vector3 destination)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsCastable(ability))
        {
            UsePositionTargetedAbility(ability, destination);
        }
    }

    protected void SendToServer_Ability_Entity(AbilityCategory abilityCategory, int abilityId, Entity target)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Entity", PhotonTargets.AllViaServer, abilityCategory, abilityId, target.EntityId, target.EntityType);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_Entity(AbilityCategory abilityCategory, int abilityId, int entityId, EntityType entityType)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsCastable(ability))
        {
            UseUnitTargetedAbility(ability, FindTarget(entityId, entityType));
        }
    }

    protected void SendToServer_Ability_Recast(AbilityCategory abilityCategory, int abilityId)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Recast", PhotonTargets.AllViaServer, abilityCategory, abilityId);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_Recast(AbilityCategory abilityCategory, int abilityId)
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

    protected void OnAbilityUsed(Ability ability)
    {
        if (ability.HasCastTime || ability.HasChannelTime || ability is DirectionTargetedDash)
        {
            currentlyUsedAbilities.Add(ability);
        }
        if (OnAnAbilityUsed != null)
        {
            OnAnAbilityUsed();
        }
    }

    protected void OnAbilityFinished(Ability ability)
    {
        if (ability.HasCastTime || ability.HasChannelTime || ability is DirectionTargetedDash)
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
        if (ability.IsEnabled && !ability.IsBlocked)
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

    protected void UsePositionTargetedAbility(Ability ability, Vector3 destination)
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
            character.CharacterBufferedAbilityManager.BufferPositionTargetedAbility(ability, destination);
        }
    }

    protected void UseUnitTargetedAbility(Ability ability, Entity target)
    {
        if (!IsUsingAbilityPreventingAbilityCast(ability))
        {
            if (currentlyUsedAbilities.Count > 0)
            {
                character.CharacterBufferedAbilityManager.ResetBufferedAbility();
            }
            ability.UseAbility(target);
        }
        else
        {
            character.CharacterBufferedAbilityManager.BufferUnitTargetedAbility(ability, target);
        }
    }

    //True: Allow ability to be cast
    //False: Act as if the key was not pressed
    protected bool AbilityIsCastable(Ability abilityToCast)
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
    protected bool IsUsingAbilityPreventingAbilityCast(Ability abilityToCast)
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

    protected bool CannotCastAbility(Ability abilityToCast, Ability ability)
    {
        return ability.HasCastTime && ability.IsBeingCasted &&
            (ability.CastableAbilitiesWhileActive == null || !CanCastAbilityWhileActive(ability.CastableAbilitiesWhileActive, abilityToCast)) &&
            !(ability.HasChannelTime && ability.IsBeingChanneled && ability.CanUseAnyAbilityWhileChanneling);
    }

    protected bool CanCastAbilityWhileActive(Ability[] castableAbilitiesWhileActive, Ability abilityToCast)
    {
        bool canCast = false;

        foreach (Ability ability in castableAbilitiesWhileActive)
        {
            if (ability && ability == abilityToCast)
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

    protected bool CanMoveWhileAbilityIsActive(Ability ability)
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

    public void StopAllChannelledAbilities()
    {
        for (int i = currentlyUsedAbilities.Count - 1; i >= 0; i--)
        {
            Ability ability = currentlyUsedAbilities[i];
            if (ability.IsBeingChanneled)
            {
                ability.CancelAbility();
            }
        }
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

    public void StopAllSpecialNonChannelledAbilities()//LucianR only so far
    {
        for (int i = currentlyUsedAbilities.Count - 1; i >= 0; i--)
        {
            Ability ability = currentlyUsedAbilities[i];
            if (ability is Lucian_R && ability.IsActive)
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

    public void SetAbilityLevelsFromLoad(int[] characterAbilityLevels)
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
