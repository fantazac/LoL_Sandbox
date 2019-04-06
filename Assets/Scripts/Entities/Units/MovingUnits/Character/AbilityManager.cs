using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Champion champion;

    public Ability[] CharacterAbilities { get; protected set; }
    public Ability[] PassiveCharacterAbilities { get; protected set; }
    public Ability[] OtherCharacterAbilities { get; protected set; }
    public Ability[] SummonerAbilities { get; protected set; }
    private Ability[] OfflineAbilities { get; set; }

    private readonly List<Ability> characterAbilitiesWithResourceCosts;
    private readonly List<Ability> currentlyUsedAbilities;

    public delegate void OnAnAbilityUsedHandler();
    public event OnAnAbilityUsedHandler OnAnAbilityUsed;

    protected AbilityManager()
    {
        currentlyUsedAbilities = new List<Ability>();
        characterAbilitiesWithResourceCosts = new List<Ability>();
    }

    protected void Awake()
    {
        champion = GetComponent<Champion>();
        InitAbilities();
        InitCharacterAbilitiesWithResourceCosts();
    }

    protected virtual void InitAbilities()
    {
        bool isLocalChampion = champion.IsLocalChampion();

        SetupAbilitiesFromAbilityCategory(CharacterAbilities, AbilityCategory.CharacterAbility, isLocalChampion);

        SetupAbilitiesFromAbilityCategory(PassiveCharacterAbilities, AbilityCategory.PassiveCharacterAbility, isLocalChampion);

        OtherCharacterAbilities = new Ability[] { gameObject.AddComponent<Recall>() };
        SetupAbilitiesFromAbilityCategory(OtherCharacterAbilities, AbilityCategory.OtherCharacterAbility, isLocalChampion);

        SetupAbilitiesFromAbilityCategory(SummonerAbilities, AbilityCategory.SummonerAbility, isLocalChampion);

        if (StaticObjects.OnlineMode) return;

        OfflineAbilities = new Ability[]
            { gameObject.AddComponent<DestroyAllDummies>(), gameObject.AddComponent<SpawnAllyDummy>(), gameObject.AddComponent<SpawnEnemyDummy>() };
        SetupAbilitiesFromAbilityCategory(OfflineAbilities, AbilityCategory.OfflineAbility, isLocalChampion);
    }

    private void SetupAbilitiesFromAbilityCategory(Ability[] abilities, AbilityCategory abilityCategory, bool isLocalCharacter)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            Ability ability = abilities[i];
            ability.OnAbilityUsed += OnAbilityUsed;
            ability.OnAbilityFinished += OnAbilityFinished;
            
            if (!isLocalCharacter) continue;
            
            ability.ID = i;
            ability.AbilityCategory = abilityCategory;
        }
    }

    private void OnAbilityUsed(Ability ability)
    {
        if (ability.HasCastTime || ability.HasChannelTime)
        {
            currentlyUsedAbilities.Add(ability);
        }

        OnAnAbilityUsed?.Invoke();
    }

    private void OnAbilityFinished(Ability ability)
    {
        if (!ability.HasCastTime && !ability.HasChannelTime) return;

        currentlyUsedAbilities.Remove(ability);

        champion.ChampionMovementManager.RotateCharacterIfMoving();

        Ability bufferedAbility = champion.BufferedAbilityManager.GetBufferedAbility();

        if (!bufferedAbility) return;

        if (HasEnoughResourceToCastAbility(bufferedAbility))
        {
            champion.MovementManager.StopMovement(false);
            champion.BufferedAbilityManager.UseBufferedAbility();
        }
        else
        {
            champion.BufferedAbilityManager.ResetBufferedAbility();
        }
    }

    private void InitCharacterAbilitiesWithResourceCosts()
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
        if (!champion.IsLocalChampion()) return;

        InitAbilityUIManager();
        if (champion.StatsManager.Resource != null)
        {
            champion.StatsManager.Resource.OnCurrentResourceChanged += OnCurrentResourceChanged;
        }
    }

    private void InitAbilityUIManager()
    {
        SetAbilitySpritesForAbilitiesFromAbilityCategory(CharacterAbilities, AbilityCategory.CharacterAbility);
        SetAbilitySpritesForAbilitiesFromAbilityCategory(PassiveCharacterAbilities, AbilityCategory.PassiveCharacterAbility);
        SetAbilitySpritesForAbilitiesFromAbilityCategory(OtherCharacterAbilities, AbilityCategory.OtherCharacterAbility);
        SetAbilitySpritesForAbilitiesFromAbilityCategory(SummonerAbilities, AbilityCategory.SummonerAbility);

        foreach (Ability characterAbility in CharacterAbilities)
        {
            champion.AbilityUIManager.DisableAbility(AbilityCategory.CharacterAbility, characterAbility.ID, false);
            champion.AbilityUIManager.SetMaxAbilityLevel(characterAbility.ID, characterAbility.MaxLevel);
        }
    }

    private void SetAbilitySpritesForAbilitiesFromAbilityCategory(Ability[] abilities, AbilityCategory abilityCategory)
    {
        foreach (Ability ability in abilities)
        {
            champion.AbilityUIManager.SetAbilitySprite(abilityCategory, ability.ID, ability.AbilitySprite);
            if (ability.CanBeRecasted)
            {
                champion.AbilityUIManager.SetAbilityRecastSprite(abilityCategory, ability.ID, ability.AbilityRecastSprite);
            }
        }
    }

    private void OnCurrentResourceChanged(float currentResourceValue)
    {
        for (int i = characterAbilitiesWithResourceCosts.Count - 1; i >= 0; i--)
        {
            UpdateAbilityHasEnoughResource(characterAbilitiesWithResourceCosts[i]);
        }
    }

    private void UpdateAbilityHasEnoughResource(Ability ability)
    {
        bool hasEnoughResourceToCastAbility = !AbilityIsAvailable(ability) || !AbilityCanBePressed(ability) || HasEnoughResourceToCastAbility(ability);
        champion.AbilityUIManager.UpdateAbilityHasEnoughResource(ability.ID, hasEnoughResourceToCastAbility);
        if (!ability.UsesResource)
        {
            characterAbilitiesWithResourceCosts.Remove(ability);
        }
    }

    public void SeAffectedTeamsForAbilities(Team team)
    {
        SetTeamsForAbilitiesFromAbilityCategory(CharacterAbilities, team);
        SetTeamsForAbilitiesFromAbilityCategory(PassiveCharacterAbilities, team);
        SetTeamsForAbilitiesFromAbilityCategory(SummonerAbilities, team);
    }

    private void SetTeamsForAbilitiesFromAbilityCategory(Ability[] abilities, Team team)
    {
        foreach (Ability ability in abilities)
        {
            ability.SetAffectedTeams(team);
        }
    }

    private void SendToServer_Ability_Destination(AbilityCategory abilityCategory, int abilityId, Vector3 destination)
    {
        champion.PhotonView.RPC(nameof(ReceiveFromServer_Ability_Destination), PhotonTargets.AllViaServer, abilityCategory, abilityId, destination);
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

    private void SendToServer_Ability_Unit(AbilityCategory abilityCategory, int abilityId, Unit target)
    {
        champion.PhotonView.RPC(nameof(ReceiveFromServer_Ability_Unit), PhotonTargets.AllViaServer, abilityCategory, abilityId, target.ID);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Unit(AbilityCategory abilityCategory, int abilityId, int unitId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsCastable(ability))
        {
            UseUnitTargetedAbility(ability, FindTarget(unitId));
        }
    }

    private void SendToServer_Ability_Auto(AbilityCategory abilityCategory, int abilityId)
    {
        champion.PhotonView.RPC(nameof(ReceiveFromServer_Ability_Auto), PhotonTargets.AllViaServer, abilityCategory, abilityId);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Auto(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (AbilityIsCastable(ability))
        {
            UseAutoTargetedAbility(ability);
        }
    }

    private void SendToServer_Ability_Recast(AbilityCategory abilityCategory, int abilityId)
    {
        champion.PhotonView.RPC(nameof(ReceiveFromServer_Ability_Recast), PhotonTargets.AllViaServer, abilityCategory, abilityId);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Recast(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        if (ability.IsReadyToBeRecasted)
        {
            ability.RecastAbility();
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

    public Unit FindTarget(int unitId)
    {
        return StaticObjects.Units[unitId];
    }

    public void LevelUpAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);
        ability.LevelUp();
        if (champion.AbilityUIManager)
        {
            champion.AbilityUIManager.LevelUpAbility(ability.ID, ability.AbilityLevel);
        }
    }

    public void OnPressedInputForAbility(AbilityCategory abilityCategory, int abilityId)
    {
        Ability ability = GetAbility(abilityCategory, abilityId);

        if (!AbilityIsAvailable(ability)) return;

        if (AbilityIsCastable(ability))
        {
            switch (ability)
            {
                case IUnitTargeted unitTargetedAbility:
                {
                    Unit hoveredUnit = champion.MouseManager.HoveredUnit;
                    if (hoveredUnit && unitTargetedAbility.CanBeCast(hoveredUnit))
                    {
                        if (StaticObjects.OnlineMode)
                        {
                            SendToServer_Ability_Unit(abilityCategory, abilityId, hoveredUnit);
                        }
                        else
                        {
                            UseUnitTargetedAbility(ability, hoveredUnit);
                        }
                    }

                    break;
                }
                case IDestinationTargeted destinationTargetedAbility when destinationTargetedAbility.CanBeCast(Input.mousePosition):
                {
                    if (StaticObjects.OnlineMode)
                    {
                        SendToServer_Ability_Destination(abilityCategory, abilityId, destinationTargetedAbility.GetDestination());
                    }
                    else
                    {
                        UsePositionTargetedAbility(ability, destinationTargetedAbility.GetDestination());
                    }

                    break;
                }
                case IAutoTargeted _:
                {
                    if (StaticObjects.OnlineMode)
                    {
                        SendToServer_Ability_Auto(abilityCategory, abilityId);
                    }
                    else
                    {
                        UseAutoTargetedAbility(ability);
                    }

                    break;
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

    private void UsePositionTargetedAbility(Ability ability, Vector3 destination)
    {
        if (AbilityCanBeCastDuringActiveAbilitiesCastTimes(ability))
        {
            champion.BufferedAbilityManager.ResetBufferedAbility();
            ((IDestinationTargeted)ability).UseAbility(destination);
            if (ability.HasCastTime || ability.HasChannelTime)
            {
                champion.ChampionMovementManager.SetCharacterIsInTargetRangeEventForBasicAttack();
            }
        }
        else
        {
            champion.BufferedAbilityManager.BufferPositionTargetedAbility(ability, destination);
        }
    }

    private void UseUnitTargetedAbility(Ability ability, Unit target)
    {
        if (AbilityCanBeCastDuringActiveAbilitiesCastTimes(ability))
        {
            champion.BufferedAbilityManager.ResetBufferedAbility();
            ((IUnitTargeted)ability).UseAbility(target);
        }
        else
        {
            champion.BufferedAbilityManager.BufferUnitTargetedAbility(ability, target);
        }
    }

    private void UseAutoTargetedAbility(Ability ability)
    {
        if (AbilityCanBeCastDuringActiveAbilitiesCastTimes(ability))
        {
            champion.BufferedAbilityManager.ResetBufferedAbility();
            ((IAutoTargeted)ability).UseAbility();
        }
        else
        {
            champion.BufferedAbilityManager.BufferAutoTargetedAbility(ability);
        }
    }

    private bool AbilityIsAvailable(Ability abilityToCast)
    {
        return abilityToCast && abilityToCast.IsEnabled && !abilityToCast.IsBlocked;
    }

    private bool AbilityIsCastable(Ability abilityToCast)
    {
        return AbilityCanBePressed(abilityToCast) && HasEnoughResourceToCastAbility(abilityToCast) && !ActiveAbilitiesPreventOtherAbilityCasts();
    }

    private bool AbilityCanBePressed(Ability abilityToCast)
    {
        return !abilityToCast.IsOnCooldown && !abilityToCast.IsOnCooldownForRecast && !abilityToCast.IsActive;
    }

    private bool HasEnoughResourceToCastAbility(Ability abilityToCast)
    {
        return !abilityToCast.UsesResource || abilityToCast.GetResourceCost() <= champion.StatsManager.Resource.GetCurrentValue();
    }

    private bool ActiveAbilitiesPreventOtherAbilityCasts()
    {
        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (ability.CannotCastAnyAbilityWhileActive)
            {
                return true;
            }
        }

        return false;
    }

    private bool AbilityCanBeCastDuringActiveAbilitiesCastTimes(Ability abilityToCast)
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

    private bool CanMoveWhileAbilityIsActive(Ability ability)
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

    public void BlockAllMovementAbilities()
    {
        BlockMovementAbilities(CharacterAbilities);
        BlockMovementAbilities(SummonerAbilities);
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

    private void BlockAbilities(Ability[] abilities)
    {
        foreach (Ability ability in abilities)
        {
            ability.BlockAbility();
        }
    }

    private void BlockMovementAbilities(Ability[] abilities)
    {
        foreach (Ability ability in abilities)
        {
            if (ability.IsAMovementAbility)
            {
                ability.BlockAbility();
            }
        }
    }

    private void UnblockAbilities(Ability[] abilities)
    {
        foreach (Ability ability in abilities)
        {
            ability.UnblockAbility();
        }
    }

    private void UnblockMovementAbilities(Ability[] abilities)
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

    public void CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled() //This is to stop LucianR in specific conditions, check if could be used for other abilities
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

    private void ResetCooldownsForAbilitiesFromAbilityCategory(Ability[] abilities, AbilityCategory abilityCategory)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            GetAbility(abilityCategory, i).ResetCooldown();
        }
    }

    public int[] GetCharacterAbilityLevels()
    {
        return new[] { CharacterAbilities[0].AbilityLevel, CharacterAbilities[1].AbilityLevel, CharacterAbilities[2].AbilityLevel, CharacterAbilities[3].AbilityLevel };
    }

    public void SetAbilityLevelsFromLoad(int[] characterAbilityLevels)
    {
        SetAbilityLevelFromLoad(CharacterAbilities[0], characterAbilityLevels[0]);
        SetAbilityLevelFromLoad(CharacterAbilities[1], characterAbilityLevels[1]);
        SetAbilityLevelFromLoad(CharacterAbilities[2], characterAbilityLevels[2]);
        SetAbilityLevelFromLoad(CharacterAbilities[3], characterAbilityLevels[3]);
    }

    private void SetAbilityLevelFromLoad(Ability ability, int abilityLevel)
    {
        for (int i = 0; i < abilityLevel; i++)
        {
            ability.LevelUp();
        }
    }
}
