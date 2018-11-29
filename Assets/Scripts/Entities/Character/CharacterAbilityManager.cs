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

    protected void Awake()
    {
        character = GetComponent<Character>();
        InitAbilities();
        InitCharacterAbilitiesWithResourceCosts();
    }

    protected virtual void InitAbilities()
    {
        bool isLocalCharacter = character.IsLocalCharacter();

        SetupAbilitiesFromAbilityCategory(CharacterAbilities, AbilityCategory.CharacterAbility, isLocalCharacter);

        SetupAbilitiesFromAbilityCategory(PassiveCharacterAbilities, AbilityCategory.PassiveCharacterAbility, isLocalCharacter);

        OtherCharacterAbilities = new Ability[] { gameObject.AddComponent<Recall>() };
        SetupAbilitiesFromAbilityCategory(OtherCharacterAbilities, AbilityCategory.OtherCharacterAbility, isLocalCharacter);

        SetupAbilitiesFromAbilityCategory(SummonerAbilities, AbilityCategory.SummonerAbility, isLocalCharacter);

        if (!StaticObjects.OnlineMode)
        {
            OfflineAbilities = new Ability[] { gameObject.AddComponent<DestroyAllDummies>(), gameObject.AddComponent<SpawnAllyDummy>(), gameObject.AddComponent<SpawnEnemyDummy>() };
            SetupAbilitiesFromAbilityCategory(OfflineAbilities, AbilityCategory.OfflineAbility, isLocalCharacter);
        }
    }

    protected void SetupAbilitiesFromAbilityCategory(Ability[] abilities, AbilityCategory abilityCategory, bool isLocalCharacter)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            Ability ability = abilities[i];
            ability.OnAbilityUsed += OnAbilityUsed;
            ability.OnAbilityFinished += OnAbilityFinished;
            if (isLocalCharacter)
            {
                ability.ID = i;
                ability.AbilityCategory = abilityCategory;
            }
        }
    }

    protected void OnAbilityUsed(Ability ability)
    {
        if (ability.HasCastTime || ability.HasChannelTime)
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
        if (ability.HasCastTime || ability.HasChannelTime)
        {
            currentlyUsedAbilities.Remove(ability);

            character.CharacterMovementManager.RotateCharacterIfMoving();

            Ability bufferedAbility = character.CharacterBufferedAbilityManager.GetBufferedAbility();
            if (bufferedAbility != null)
            {
                if (HasEnoughResourceToCastAbility(bufferedAbility))
                {
                    character.CharacterMovementManager.StopAllMovement(false);
                    character.CharacterBufferedAbilityManager.UseBufferedAbility();
                }
                else
                {
                    character.CharacterBufferedAbilityManager.ResetBufferedAbility();
                }
            }
        }
    }

    protected void InitCharacterAbilitiesWithResourceCosts()
    {
        foreach (Ability characterAbility in CharacterAbilities)
        {
            if (characterAbility.UsesResource)
            {
                characterAbilitiesWithResourceCosts.Add(characterAbility);
            }
        }
    }

    protected void Start()
    {
        if (character.IsLocalCharacter())
        {
            InitAbilityUIManager();
            if (character.StatsManager.Resource != null)
            {
                character.StatsManager.Resource.OnCurrentResourceChanged += OnCurrentResourceChanged;
            }
        }
    }

    protected void InitAbilityUIManager()
    {
        SetAbilitySpritesForAbilitiesFromAbilityCategory(CharacterAbilities, AbilityCategory.CharacterAbility);
        SetAbilitySpritesForAbilitiesFromAbilityCategory(PassiveCharacterAbilities, AbilityCategory.PassiveCharacterAbility);
        SetAbilitySpritesForAbilitiesFromAbilityCategory(OtherCharacterAbilities, AbilityCategory.OtherCharacterAbility);
        SetAbilitySpritesForAbilitiesFromAbilityCategory(SummonerAbilities, AbilityCategory.SummonerAbility);

        foreach (Ability characterAbility in CharacterAbilities)
        {
            character.AbilityUIManager.DisableAbility(AbilityCategory.CharacterAbility, characterAbility.ID, false);
            character.AbilityUIManager.SetMaxAbilityLevel(characterAbility.ID, characterAbility.MaxLevel);
        }
    }

    protected void SetAbilitySpritesForAbilitiesFromAbilityCategory(Ability[] abilities, AbilityCategory abilityCategory)
    {
        foreach (Ability ability in abilities)
        {
            character.AbilityUIManager.SetAbilitySprite(abilityCategory, ability.ID, ability.AbilitySprite);
        }
    }

    protected void OnCurrentResourceChanged(float currentResourceValue)
    {
        for (int i = characterAbilitiesWithResourceCosts.Count - 1; i >= 0; i--)
        {
            UpdateAbilityHasEnoughResource(characterAbilitiesWithResourceCosts[i], currentResourceValue);
        }
    }

    protected void UpdateAbilityHasEnoughResource(Ability ability, float currentValue)
    {
        bool hasEnoughResourceToCastAbility = !AbilityIsAvailable(ability) || !AbilityCanBePressed(ability) || HasEnoughResourceToCastAbility(ability);
        character.AbilityUIManager.UpdateAbilityHasEnoughResource(ability.ID, hasEnoughResourceToCastAbility);
        if (!ability.UsesResource)
        {
            characterAbilitiesWithResourceCosts.Remove(ability);
        }
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
        if (ability.IsReadyToBeRecasted)
        {
            ability.RecastAbility();
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

    public void LevelUpAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        ability.LevelUp();
        if (character.AbilityUIManager)
        {
            character.AbilityUIManager.LevelUpAbility(ability.ID, ability.AbilityLevel);
        }
    }

    public void OnPressedInputForAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsAvailable(ability))
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
            else if (ability.IsReadyToBeRecasted)
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
        if (AbilityCanBeCastDuringActiveAbilitiesCastTimes(ability))
        {
            character.CharacterBufferedAbilityManager.ResetBufferedAbility();
            ability.UseAbility(destination);
            if (ability.HasCastTime || ability.HasChannelTime)
            {
                character.CharacterMovementManager.SetCharacterIsInTargetRangeEventForBasicAttack();
            }
        }
        else
        {
            character.CharacterBufferedAbilityManager.BufferPositionTargetedAbility(ability, destination);
        }
    }

    protected void UseUnitTargetedAbility(Ability ability, Entity target)
    {
        if (AbilityCanBeCastDuringActiveAbilitiesCastTimes(ability))
        {
            character.CharacterBufferedAbilityManager.ResetBufferedAbility();
            ability.UseAbility(target);
        }
        else
        {
            character.CharacterBufferedAbilityManager.BufferUnitTargetedAbility(ability, target);
        }
    }

    protected bool AbilityIsAvailable(Ability abilityToCast)
    {
        return abilityToCast.IsEnabled && !abilityToCast.IsBlocked;
    }

    protected bool AbilityIsCastable(Ability abilityToCast)
    {
        return AbilityCanBePressed(abilityToCast) && HasEnoughResourceToCastAbility(abilityToCast) && AbilityIsAllowedToBeCastWhileOtherAbilitiesAreActive(abilityToCast);
    }

    protected bool AbilityCanBePressed(Ability abilityToCast)
    {
        return !abilityToCast.IsOnCooldown && !abilityToCast.IsOnCooldownForRecast && !abilityToCast.IsActive;
    }

    protected bool HasEnoughResourceToCastAbility(Ability abilityToCast)
    {
        return !abilityToCast.UsesResource || abilityToCast.GetResourceCost() <= character.StatsManager.Resource.GetCurrentValue();
    }

    protected bool AbilityIsAllowedToBeCastWhileOtherAbilitiesAreActive(Ability abilityToCast)
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.CannotCastAnyAbilityWhileActive)
            {
                return false;
            }
        }

        return true;
    }

    protected bool AbilityCanBeCastDuringActiveAbilitiesCastTimes(Ability abilityToCast)
    {
        return abilityToCast.CanBeCastDuringOtherAbilityCastTimes || !AnAbilityIsBeingCasted();
    }

    public bool AnAbilityIsBeingCasted()
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.IsBeingCasted)
            {
                return true;
            }
        }

        return false;
    }

    public bool CanUseMovement()
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!CanMoveWhileAbilityIsActive(ability))
            {
                return false;
            }
        }

        return true;
    }

    protected bool CanMoveWhileAbilityIsActive(Ability ability)
    {
        return ability.CanMoveWhileActive || (ability.CanMoveWhileChanneling && ability.IsBeingChanneled);
    }

    public bool CanUseBasicAttacks()
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!ability.CanUseBasicAttacksWhileCasting)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanRotate()
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.CannotRotateWhileActive)
            {
                return false;
            }
        }

        return true;
    }

    public bool AnAbilityInBeingChanneled()
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.IsBeingChanneled)
            {
                return true;
            }
        }

        return false;
    }

    public void BlockAllBasicAbilities()
    {
        BlockAbilities(CharacterAbilities);
        BlockMovementAbilities(SummonerAbilities);
        BlockAbilities(OtherCharacterAbilities);
    }

    public void BlockAllMovementAbilities(Ability abilityToNotBlock = null)
    {
        BlockMovementAbilities(CharacterAbilities, abilityToNotBlock);
        BlockMovementAbilities(SummonerAbilities, abilityToNotBlock);
    }

    public void BlockAllSummonerAbilities()
    {
        BlockAbilities(SummonerAbilities);
    }

    public void UnblockAllBasicAbilities(bool canUseMovementAbilities, bool canUseSummonerAbilities)
    {
        foreach (Ability ability in CharacterAbilities)
        {
            if (!ability.IsAMovementAbility || canUseMovementAbilities)
            {
                ability.UnblockAbility();
            }
        }
        foreach (Ability ability in SummonerAbilities)
        {
            if (canUseSummonerAbilities && (!ability.IsAMovementAbility || canUseMovementAbilities))
            {
                ability.UnblockAbility();
            }
        }
        UnblockAbilities(OtherCharacterAbilities);
    }

    public void UnblockAllMovementAbilities()
    {
        UnblockMovementAbilities(CharacterAbilities);
        UnblockMovementAbilities(SummonerAbilities);
    }

    public void UnblockAllSummonerAbilities(bool canUseMovementAbilities)
    {
        foreach (Ability ability in SummonerAbilities)
        {
            if (!ability.IsAMovementAbility || canUseMovementAbilities)
            {
                ability.UnblockAbility();
            }
        }
    }

    protected void BlockAbilities(Ability[] abilities)
    {
        foreach (Ability ability in abilities)
        {
            ability.BlockAbility();
        }
    }

    protected void BlockMovementAbilities(Ability[] abilities, Ability abilityToNotBlock = null)
    {
        foreach (Ability ability in abilities)
        {
            if (ability.IsAMovementAbility && ability != abilityToNotBlock)
            {
                ability.BlockAbility();
            }
        }
    }

    protected void UnblockAbilities(Ability[] abilities)
    {
        foreach (Ability ability in abilities)
        {
            ability.UnblockAbility();
        }
    }

    protected void UnblockMovementAbilities(Ability[] abilities)
    {
        foreach (Ability ability in abilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.UnblockAbility();
            }
        }
    }

    public void CancelAllChannelingAbilities()
    {
        for (int i = currentlyUsedAbilities.Count - 1; i >= 0; i--)
        {
            Ability ability = currentlyUsedAbilities[i];
            if (ability.IsBeingChanneled && ability.CannotCancelChannel)
            {
                ability.CancelAbility();
            }
        }
    }

    public void CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled()//This is to stop LucianR in specific conditions, check if could be used for other abilities
    {
        for (int i = currentlyUsedAbilities.Count - 1; i >= 0; i--)
        {
            Ability ability = currentlyUsedAbilities[i];
            if (!ability.IsBeingCasted && !ability.IsBeingChanneled && ability.IsActive)
            {
                ability.CancelAbility();
            }
        }
    }

    public void ResetCooldowns()
    {
        ResetCooldownsForAbilitiesFromAbilityCategory(CharacterAbilities, AbilityCategory.CharacterAbility);
        ResetCooldownsForAbilitiesFromAbilityCategory(PassiveCharacterAbilities, AbilityCategory.PassiveCharacterAbility);
        ResetCooldownsForAbilitiesFromAbilityCategory(OtherCharacterAbilities, AbilityCategory.OtherCharacterAbility);
        ResetCooldownsForAbilitiesFromAbilityCategory(SummonerAbilities, AbilityCategory.SummonerAbility);
        if (!StaticObjects.OnlineMode)
        {
            ResetCooldownsForAbilitiesFromAbilityCategory(OfflineAbilities, AbilityCategory.OfflineAbility);
        }
    }

    protected void ResetCooldownsForAbilitiesFromAbilityCategory(Ability[] abilities, AbilityCategory abilityCategory)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            GetAbility(abilityCategory, i).ResetCooldown();
        }
    }

    public int[] GetCharacterAbilityLevels()
    {
        return new int[] { CharacterAbilities[0].AbilityLevel, CharacterAbilities[1].AbilityLevel, CharacterAbilities[2].AbilityLevel, CharacterAbilities[3].AbilityLevel };
    }

    public void SetAbilityLevelsFromLoad(int[] characterAbilityLevels)
    {
        SetAbilityLevelFromLoad(CharacterAbilities[0], characterAbilityLevels[0]);
        SetAbilityLevelFromLoad(CharacterAbilities[1], characterAbilityLevels[1]);
        SetAbilityLevelFromLoad(CharacterAbilities[2], characterAbilityLevels[2]);
        SetAbilityLevelFromLoad(CharacterAbilities[3], characterAbilityLevels[3]);
    }

    protected void SetAbilityLevelFromLoad(Ability ability, int abilityLevel)
    {
        for (int i = 0; i < abilityLevel; i++)
        {
            ability.LevelUp();
        }
    }
}
