using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : DamageSource
{
    protected Champion champion;

    protected List<Team> affectedTeams;
    protected List<Type> affectedUnitTypes;
    protected AbilityEffectType effectType;
    protected AbilityType abilityType;

    private IEnumerator abilityEffectCoroutine;
    private IEnumerator cooldownForRecastCoroutine;

    public AbilityCategory AbilityCategory { get; set; }
    public int AbilityLevel { get; protected set; }
    public int ID { get; set; }
    public int MaxLevel { get; protected set; }
    public bool IsAnUltimateAbility { get; protected set; }

    protected string abilityName;
    protected float castTime;
    protected float channelTime;
    protected float cooldownBeforeRecast;
    protected float cooldownRemaining;
    protected Vector3 destinationOnCast;
    protected WaitForSeconds delayCastTime;
    protected WaitForSeconds delayChannelTime;
    protected RaycastHit hit;
    protected Vector3 positionOnCast;
    protected float range;
    protected Quaternion rotationOnCast;
    protected float speed;
    protected bool startCooldownOnAbilityCast;
    protected Unit targetedUnit;

    protected bool affectedByCooldownReduction;
    protected float baseCooldown;
    protected float baseCooldownOnCancel;
    protected float baseCooldownPerLevel;
    protected float bonusADScaling;
    protected float bonusADScalingPerLevel;
    private float cooldown;
    private float cooldownOnCancel;
    protected float damage;
    protected float damagePerLevel;
    protected float resourceCost;
    protected float resourceCostPerLevel;
    protected float totalADScaling;
    protected float totalADScalingPerLevel;
    protected float totalAPScaling;
    protected float totalAPScalingPerLevel;

    public Sprite AbilitySprite { get; private set; }
    public Sprite AbilityRecastSprite { get; private set; }//TODO: Not implemented

    protected string abilitySpritePath;
    protected string abilityRecastSpritePath;

    private int abilityIsBlockedCount;

    public bool AppliesAbilityEffects { get; protected set; }
    public bool AppliesOnHitEffects { get; protected set; }
    public bool CanBeCastDuringOtherAbilityCastTimes { get; protected set; }
    public bool CanBeCastWhileStunned { get; protected set; }
    public bool CanBeRecasted { get; protected set; }
    public bool CanMoveWhileActive { get; protected set; }
    public bool CanMoveWhileChanneling { get; protected set; }
    public bool CannotCancelChannel { get; protected set; }
    public bool CannotCastAnyAbilityWhileActive { get; protected set; }
    public bool CannotRotateWhileActive { get; protected set; }
    public bool CanUseAnyAbilityWhileChanneling { get; protected set; }
    public bool CanUseBasicAttacksWhileCasting { get; protected set; }
    public bool HasCastTime { get; private set; }
    public bool HasChannelTime { get; private set; }
    public bool HasReducedCooldownOnAbilityCancel { get; private set; }
    public bool IsActive { get; protected set; }
    public bool IsAMovementAbility { get; protected set; }
    public bool IsBeingCasted { get; protected set; }
    public bool IsBeingChanneled { get; protected set; }
    public bool IsBlocked { get; protected set; }
    public bool IsEnabled { get; protected set; }
    public bool IsOnCooldown { get; protected set; }
    public bool IsOnCooldownForRecast { get; protected set; }
    public bool IsReadyToBeRecasted { get; private set; }
    public bool OfflineOnly { get; protected set; }
    public bool ResetBasicAttackCycleOnAbilityCast { get; protected set; }
    public bool ResetBasicAttackCycleOnAbilityFinished { get; protected set; }
    public bool UsesResource { get; private set; }

    public List<Ability> AbilitiesToDisableWhileActive { get; private set; }
    public List<Ability> CastableAbilitiesWhileActive { get; private set; }

    public AbilityBuff[] AbilityBuffs { get; protected set; }
    public AbilityBuff[] AbilityDebuffs { get; protected set; }

    public delegate void OnAbilityUsedHandler(Ability ability);
    public event OnAbilityUsedHandler OnAbilityUsed;

    public delegate void OnAbilityFinishedHandler(Ability ability);
    public event OnAbilityFinishedHandler OnAbilityFinished;

    public delegate void OnAbilityHitHandler(Ability ability, Unit unitHit);
    public event OnAbilityHitHandler OnAbilityHit;

    protected Ability()
    {
        AppliesAbilityEffects = true;//TODO: Certaines abilities appliquent pas ca (ex. darius w) mais pourquoi?

        AbilitiesToDisableWhileActive = new List<Ability>();
        CastableAbilitiesWhileActive = new List<Ability>();

        SetResourcePaths();
    }

    protected virtual void Awake()
    {
        champion = GetComponent<Champion>();
        if (champion.IsLocalChampion())
        {
            LoadSprites();
        }
        LoadPrefabs();

        HasCastTime = castTime > 0;
        HasChannelTime = channelTime > 0;
        HasReducedCooldownOnAbilityCancel = baseCooldownOnCancel > 0;
        UsesResource = resourceCost > 0;
    }

    protected virtual void Start()
    {
        if (champion.AbilityUIManager && UsesResource)
        {
            champion.AbilityUIManager.SetAbilityCost(ID, resourceCost);
        }
        if (affectedByCooldownReduction)
        {
            champion.StatsManager.CooldownReduction.OnCooldownReductionChanged += SetCooldownForAbilityAffectedByCooldownReduction;
            SetCooldownForAbilityAffectedByCooldownReduction();
        }
        else
        {
            SetCooldownForAbilityUnaffectedByCooldownReduction();
        }

        ModifyValues();
    }

    public abstract bool CanBeCast(Unit target);
    public abstract bool CanBeCast(Vector3 mousePosition);
    public abstract Vector3 GetDestination();
    public abstract void UseAbility(Unit target);
    public abstract void UseAbility(Vector3 destination);

    protected abstract void SetResourcePaths();

    protected virtual void RotationOnAbilityCast(Vector3 destination)
    {
        champion.OrientationManager.RotateCharacterTowardsCastPoint(destination);
    }

    protected virtual void ModifyValues()
    {
        range *= StaticObjects.MultiplyingFactor;
        speed *= StaticObjects.MultiplyingFactor;
    }

    private void LoadSprites()
    {
        AbilitySprite = Resources.Load<Sprite>(abilitySpritePath);
        AbilityRecastSprite = Resources.Load<Sprite>(abilityRecastSpritePath);
    }

    protected virtual void LoadPrefabs() { }

    protected void StartAbilityCast()
    {
        if (AbilitiesToDisableWhileActive != null)
        {
            foreach (Ability ability in AbilitiesToDisableWhileActive)
            {
                if (ability && ability.AbilityLevel > 0)
                {
                    ability.DisableAbility();
                }
            }
        }
        if (CanBeRecasted)
        {
            StartCooldownForRecast();
        }
        if (OnAbilityUsed != null)
        {
            OnAbilityUsed(this);
        }
        IsActive = true;
        if (champion.AbilityTimeBarUIManager)
        {
            if (HasCastTime && HasChannelTime)
            {
                champion.AbilityTimeBarUIManager.SetCastTimeAndChannelTime(castTime, channelTime, abilityName, ID);
            }
            else if (HasCastTime)
            {
                champion.AbilityTimeBarUIManager.SetCastTime(castTime, abilityName, ID);
            }
            else if (HasChannelTime)
            {
                champion.AbilityTimeBarUIManager.SetChannelTime(channelTime, abilityName, ID);
            }
        }
        if (ResetBasicAttackCycleOnAbilityCast)
        {
            champion.BasicAttack.ResetBasicAttack();
        }
        StartCooldown(true);
    }

    protected void UseResource()
    {
        if (UsesResource)
        {
            champion.StatsManager.Resource.Reduce(resourceCost);
        }
    }

    protected void FinishAbilityCast(bool abilityWasCancelled = false)
    {
        abilityEffectCoroutine = null;
        IsReadyToBeRecasted = false;
        if (AbilitiesToDisableWhileActive != null)
        {
            foreach (Ability ability in AbilitiesToDisableWhileActive)
            {
                if (ability)
                {
                    ability.EnableAbility();
                }
            }
        }
        if (OnAbilityFinished != null)
        {
            OnAbilityFinished(this);
        }
        IsActive = false;
        if (ResetBasicAttackCycleOnAbilityFinished)
        {
            champion.BasicAttack.ResetBasicAttack();
        }
        StartCooldown(false, abilityWasCancelled);
    }

    public void DisableAbility()
    {
        IsEnabled = false;
        if (!IsOnCooldown && champion.AbilityUIManager)
        {
            champion.AbilityUIManager.DisableAbility(AbilityCategory, ID, UsesResource);
        }
    }

    public virtual void EnableAbility()
    {
        if (AbilityLevel > 0)
        {
            IsEnabled = true;
            if (!IsOnCooldown && !IsActive && champion.AbilityUIManager)
            {
                if (IsBlocked)
                {
                    BlockAbility();
                }
                else
                {
                    champion.AbilityUIManager.EnableAbility(AbilityCategory, ID, resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
                }
            }
        }
    }

    public void BlockAbility()
    {
        abilityIsBlockedCount++;
        if (!IsBlocked)
        {
            IsBlocked = true;
            if (!IsOnCooldown && IsEnabled && champion.AbilityUIManager)
            {
                champion.AbilityUIManager.BlockAbility(AbilityCategory, ID, UsesResource);
            }
        }
    }

    public void UnblockAbility()
    {
        if (IsBlocked)
        {
            if (--abilityIsBlockedCount == 0)
            {
                IsBlocked = false;
                if (!IsOnCooldown && IsEnabled && champion.AbilityUIManager)
                {
                    champion.AbilityUIManager.UnblockAbility(AbilityCategory, ID, resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
                }
            }
        }
    }

    public virtual void EnableAbilityPassive() { }

    protected void AbilityHit(Unit unitHit, float damage, bool callOnAbilityHit = true)
    {
        if (damage > 0)
        {
            if (AppliesAbilityEffects)
            {
                champion.AbilityEffectsManager.ApplyAbilityEffectsToUnitHit(unitHit, damage);
            }
            if (AppliesOnHitEffects)
            {
                champion.OnHitEffectsManager.ApplyOnHitEffectsToUnitHit(unitHit, damage);
            }
        }
        unitHit.EffectSourceManager.UnitHitByAbility(this);
        if (OnAbilityHit != null && callOnAbilityHit)
        {
            OnAbilityHit(this, unitHit);
        }
    }

    private void StartCooldown(bool calledInStartAbilityCast, bool abilityWasCancelled = false)
    {
        if (champion.AbilityUIManager)
        {
            float abilityCooldown = abilityWasCancelled ? cooldownOnCancel : cooldown;
            if (calledInStartAbilityCast == startCooldownOnAbilityCast && abilityCooldown > 0)
            {
                if (cooldownForRecastCoroutine != null)
                {
                    StopCoroutine(cooldownForRecastCoroutine);
                }
                StartCoroutine(PutAbilityOffCooldown(abilityCooldown));
            }
        }
    }

    protected void StartCooldownForRecast()
    {
        if (champion.AbilityUIManager)
        {
            cooldownForRecastCoroutine = PutAbilityOffCooldownForRecast();
            StartCoroutine(cooldownForRecastCoroutine);
        }
    }

    //This method is used for projectiles that have their origin at the Character's origin. This means you cannot move the origin of the projectile by flashing mid-cast.
    //Examples: EzrealR, MorganaQ, CorkiR
    protected void SetPositionAndRotationOnCast(Vector3 position)
    {
        positionOnCast = position;
        rotationOnCast = Quaternion.LookRotation((destinationOnCast - transform.position).normalized);
    }

    protected virtual IEnumerator PutAbilityOffCooldown(float cooldownOnStart)
    {
        IsOnCooldown = true;
        cooldownRemaining = cooldownOnStart;

        yield return null;

        champion.AbilityUIManager.SetAbilityOnCooldown(AbilityCategory, ID, IsBlocked);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            champion.AbilityUIManager.UpdateAbilityCooldown(AbilityCategory, ID, cooldownOnStart, cooldownRemaining);

            yield return null;
        }

        champion.AbilityUIManager.SetAbilityOffCooldown(AbilityCategory, ID, UsesResource, IsEnabled, IsBlocked);
        if (UsesResource && IsEnabled && !IsBlocked)
        {
            champion.AbilityUIManager.UpdateAbilityHasEnoughResource(ID, resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
        }
        cooldownForRecastCoroutine = null;
        IsOnCooldown = false;
    }

    protected virtual IEnumerator PutAbilityOffCooldownForRecast()
    {
        IsOnCooldownForRecast = true;
        cooldownRemaining = cooldownBeforeRecast;

        yield return null;

        champion.AbilityUIManager.SetAbilityOnCooldownForRecast(AbilityCategory, ID);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            champion.AbilityUIManager.UpdateAbilityCooldownForRecast(AbilityCategory, ID, cooldownBeforeRecast, cooldownRemaining);

            yield return null;
        }

        champion.AbilityUIManager.SetAbilityOffCooldownForRecast(AbilityCategory, ID);
        cooldownForRecastCoroutine = null;
        IsOnCooldownForRecast = false;
        IsReadyToBeRecasted = true;
    }

    public void ResetCooldown()
    {
        cooldownRemaining = 0;
    }

    public void ReduceCooldown(float cooldownReductionAmount)
    {
        cooldownRemaining -= cooldownReductionAmount;
    }

    public void LevelUp()
    {
        if (AbilityLevel > 0 && AbilityLevel < MaxLevel)
        {
            AbilityLevel++;

            LevelUpExtraStats();
            LevelUpAbilityStats();
            LevelUpBuffsAndDebuffs();

            AbilityUIManager abilityUIManager = champion.AbilityUIManager;

            if (UsesResource && resourceCost == 0)
            {
                UsesResource = false;
                if (abilityUIManager)
                {
                    abilityUIManager.SetAbilityCost(ID, resourceCost);
                }
            }
            if (baseCooldownPerLevel != 0)
            {
                if (affectedByCooldownReduction)
                {
                    SetCooldownForAbilityAffectedByCooldownReduction();
                }
                else
                {
                    SetCooldownForAbilityUnaffectedByCooldownReduction();
                }
            }

            if (UsesResource && abilityUIManager)
            {
                abilityUIManager.SetAbilityCost(ID, resourceCost);
                abilityUIManager.UpdateAbilityHasEnoughResource(ID, !IsEnabled || IsBlocked || IsOnCooldown || resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
            }
        }
        else if (AbilityLevel == 0)
        {
            AbilityLevel++;
            EnableAbility();
            EnableAbilityPassive();
        }
    }

    protected void LevelUpAbilityStats()
    {
        bonusADScaling += bonusADScalingPerLevel;
        baseCooldown += baseCooldownPerLevel;
        damage += damagePerLevel;
        resourceCost += resourceCostPerLevel;
        totalADScaling += totalADScalingPerLevel;
        totalAPScaling += totalAPScalingPerLevel;
    }

    private void SetCooldownForAbilityAffectedByCooldownReduction()
    {
        SetCooldownForAbilityAffectedByCooldownReduction(champion.StatsManager.CooldownReduction.GetTotal());
    }

    protected virtual void SetCooldownForAbilityAffectedByCooldownReduction(float cooldownReduction)
    {
        cooldown = baseCooldown * (1 - cooldownReduction);
        cooldownOnCancel = baseCooldownOnCancel * (1 - cooldownReduction);
    }

    private void SetCooldownForAbilityUnaffectedByCooldownReduction()
    {
        cooldown = baseCooldown;
        cooldownOnCancel = baseCooldownOnCancel;
    }

    protected virtual float GetAbilityDamage(Unit unitHit, bool isACriticalStrike = false, float criticalStrikeDamage = 0)
    {
        float abilityDamage = damage +
            (bonusADScaling * champion.StatsManager.AttackDamage.GetBonus()) +
            (totalADScaling * champion.StatsManager.AttackDamage.GetTotal()) +
            (totalAPScaling * champion.StatsManager.AbilityPower.GetTotal());

        return ApplyDamageModifiers(unitHit, abilityDamage, damageType) *
            (isACriticalStrike ? criticalStrikeDamage : 1f);
    }

    protected virtual float ApplyAbilityDamageModifier(Unit unitHit)
    {
        return 1f;
    }

    protected virtual float ApplyDamageModifiers(Unit unitHit, float damage, DamageType damageType)
    {
        damage *= ApplyAbilityDamageModifier(unitHit);
        if (damageType == DamageType.MAGIC)
        {
            float totalResistance = unitHit.StatsManager.MagicResistance.GetTotal();
            totalResistance *= (1 - champion.StatsManager.MagicPenetrationPercent.GetTotal());
            totalResistance -= champion.StatsManager.MagicPenetrationFlat.GetTotal();
            return damage * GetResistanceDamageReceivedModifier(totalResistance) * unitHit.StatsManager.MagicDamageReceivedModifier.GetTotal() * champion.StatsManager.MagicDamageModifier.GetTotal();
        }
        else if (damageType == DamageType.PHYSICAL)
        {
            float totalResistance = unitHit.StatsManager.Armor.GetTotal();
            totalResistance *= (1 - champion.StatsManager.ArmorPenetrationPercent.GetTotal());
            totalResistance -= champion.StatsManager.Lethality.GetCurrentValue();
            return damage * GetResistanceDamageReceivedModifier(totalResistance) * unitHit.StatsManager.PhysicalDamageReceivedModifier.GetTotal() * champion.StatsManager.PhysicalDamageModifier.GetTotal();
        }

        return damage;
    }

    protected float GetResistanceDamageReceivedModifier(float totalResistance)
    {
        if (totalResistance >= 0)
        {
            return 100 / (100 + totalResistance);
        }
        else
        {
            return 2 - (100 / (100 - totalResistance));
        }
    }

    protected void LevelUpBuffsAndDebuffs()
    {
        if (AbilityBuffs != null)
        {
            foreach (AbilityBuff aBuff in AbilityBuffs)
            {
                aBuff.LevelUp();
            }
        }
        if (AbilityDebuffs != null)
        {
            foreach (AbilityBuff aDebuff in AbilityDebuffs)
            {
                aDebuff.LevelUp();
            }
        }
    }

    public virtual void LevelUpExtraStats() { }

    public virtual void OnCharacterLevelUp(int level) { }

    public virtual void RecastAbility()
    {
        CancelAbility();
    }

    public void CancelAbility()
    {
        if (abilityEffectCoroutine != null)
        {
            StopCoroutine(abilityEffectCoroutine);
        }
        ExtraActionsOnCancel();
        if (champion.AbilityTimeBarUIManager && (HasCastTime || HasChannelTime))
        {
            champion.AbilityTimeBarUIManager.CancelCastTimeAndChannelTime(ID);
        }
        FinishAbilityCast(HasReducedCooldownOnAbilityCancel);
    }

    protected virtual void ExtraActionsOnCancel() { }

    public virtual void OnEmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike = false) { }

    public void SetRange(float range)
    {
        this.range = range * StaticObjects.MultiplyingFactor;
    }

    public float GetResourceCost()
    {
        return resourceCost;
    }

    public AbilityType GetAbilityType()
    {
        return abilityType;
    }

    protected void StartCorrectCoroutine()
    {
        if (delayCastTime != null && delayChannelTime != null)
        {
            abilityEffectCoroutine = AbilityWithCastTimeAndChannelTime();
        }
        else if (delayCastTime != null)
        {
            abilityEffectCoroutine = AbilityWithCastTime();
        }
        else if (delayChannelTime != null)
        {
            abilityEffectCoroutine = AbilityWithChannelTime();
        }
        else
        {
            abilityEffectCoroutine = AbilityWithoutDelay();
        }
        StartCoroutine(abilityEffectCoroutine);
    }

    protected virtual IEnumerator AbilityWithCastTime() { yield return null; }
    protected virtual IEnumerator AbilityWithCastTimeAndChannelTime() { yield return null; }
    protected virtual IEnumerator AbilityWithChannelTime() { yield return null; }
    protected virtual IEnumerator AbilityWithoutDelay() { yield return null; }
}

