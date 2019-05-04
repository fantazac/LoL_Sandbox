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

    protected IEnumerator abilityEffectCoroutine;
    private IEnumerator cooldownForRecastCoroutine;

    public AbilityCategory AbilityCategory { get; set; }
    public int AbilityLevel { get; protected set; }
    public int ID { get; set; }
    public int MaxLevel { get; protected set; }
    public bool IsAnUltimateAbility { get; protected set; }

    protected string abilityName;
    protected bool affectedByCooldownReduction;
    protected float castTime;
    protected float channelTime;
    protected float chargeTime;
    protected float cooldownBeforeRecast;
    protected float cooldownRemaining;
    protected Vector3 destinationOnCast;
    protected WaitForSeconds delayCastTime;
    protected WaitForSeconds delayChannelTime;
    protected WaitForSeconds delayChargeTime;
    private bool hasReducedCooldownOnAbilityCancel;
    protected RaycastHit hit;
    private bool isDisabledByOutsideSource;
    private bool isEnabled;
    protected float maximumChargeTime;
    protected Vector3 positionOnCast;
    protected float range;
    protected bool resetBasicAttackCycleOnAbilityCast;
    protected bool resetBasicAttackCycleOnAbilityFinished;
    protected Quaternion rotationOnCast;
    protected float speed;
    protected bool startCooldownOnAbilityCast;
    protected Unit targetedUnit;

    protected float baseCooldown;
    protected float baseCooldownOnCancel;
    protected float baseCooldownPerLevel;
    protected float bonusADScaling;
    protected float bonusADScalingPerLevel;
    protected float damage;
    protected float damagePerLevel;
    protected float resourceCost;
    protected float resourceCostPerLevel;
    protected float totalADScaling;
    protected float totalADScalingPerLevel;
    protected float totalAPScaling;
    protected float totalAPScalingPerLevel;

    private float cooldown;
    private float cooldownOnCancel;
    
    public Sprite AbilitySprite { get; private set; }
    public Sprite AbilityRecastSprite { get; private set; }

    protected string abilitySpritePath;
    protected string abilityRecastSpritePath;

    private int abilityIsBlockedCount;

    private readonly bool appliesAbilityEffects;
    protected bool appliesOnHitEffects;
    
    public bool CanBeCastDuringOtherAbilityCastTimes { get; protected set; }
    public bool CanBeRecasted { get; protected set; }
    public bool CanMoveWhileActive { get; protected set; }
    public bool CanMoveWhileChanneling { get; protected set; }
    public bool CanMoveWhileCharging { get; protected set; }
    public bool CannotCancelChannel { get; protected set; }
    public bool CannotCancelCharge { get; protected set; }
    public bool CannotCastAnyAbilityWhileActive { get; protected set; }
    public bool CannotRotateWhileActive { get; protected set; }
    public bool CanUseBasicAttacksWhileCasting { get; protected set; }
    public bool HasCastTime { get; private set; }
    public bool HasChannelTime { get; private set; }
    public bool HasChargeTime { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsAMovementAbility { get; protected set; }
    public bool IsBeingCasted { get; protected set; }
    public bool IsBeingChanneled { get; protected set; }
    public bool IsBeingCharged { get; protected set; }
    public bool IsBlocked { get; private set; }
    public bool IsOnCooldown { get; private set; }
    public bool IsOnCooldownForRecast { get; private set; }
    public bool IsReadyToBeRecasted { get; private set; }
    public bool UsesResource { get; private set; }

    protected readonly List<Ability> abilitiesToDisableWhileActive;

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
        appliesAbilityEffects = true; //TODO: Certaines abilities appliquent pas ca (ex. darius w) mais pourquoi?
        startCooldownOnAbilityCast = false; //Maybe a skill uses this?

        abilitiesToDisableWhileActive = new List<Ability>();
    }

    protected virtual void Awake()
    {
        SetResourcePaths();
        
        champion = GetComponent<Champion>();
        if (champion.IsLocalChampion())
        {
            LoadSprites();
        }

        LoadPrefabs();

        HasCastTime = castTime > 0;
        HasChannelTime = channelTime > 0;
        HasChargeTime = chargeTime > 0;
        hasReducedCooldownOnAbilityCancel = baseCooldownOnCancel > 0;
        isEnabled = AbilityLevel > 0;
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

    public bool IsEnabled()
    {
        return isEnabled && !isDisabledByOutsideSource;
    }
    
    protected void StartAbilityCast()
    {
        if (abilitiesToDisableWhileActive != null)
        {
            foreach (Ability ability in abilitiesToDisableWhileActive)
            {
                ability.DisableAbility();
            }
        }

        if (CanBeRecasted)
        {
            StartCooldownForRecast();
        }

        OnAbilityUsed?.Invoke(this);

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
            else if (HasChargeTime)
            {
                champion.AbilityTimeBarUIManager.SetChargeTime(chargeTime, maximumChargeTime, abilityName, ID);
            }
        }

        if (resetBasicAttackCycleOnAbilityCast)
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
        if (abilitiesToDisableWhileActive != null)
        {
            foreach (Ability ability in abilitiesToDisableWhileActive)
            {
                if (ability)
                {
                    ability.EnableAbility(false);
                }
            }
        }

        OnAbilityFinished?.Invoke(this);

        IsActive = false;
        if (champion.AbilityTimeBarUIManager && (HasCastTime || HasChannelTime || HasChargeTime))
        {
            champion.AbilityTimeBarUIManager.CancelCastTimeAndChannelTimeAndChargeTime(ID);
        }
        if (resetBasicAttackCycleOnAbilityFinished)
        {
            champion.BasicAttack.ResetBasicAttack();
        }

        StartCooldown(false, abilityWasCancelled);
    }
    
    private void DisableAbility()
    {
        isDisabledByOutsideSource = true;
        if (!IsOnCooldown && champion.AbilityUIManager)
        {
            champion.AbilityUIManager.DisableAbility(AbilityCategory, ID, UsesResource);
        }
    }

    protected void DisableOtherAbility(Ability ability)
    {
        ability.DisableAbility();
    }

    private void EnableAbility(bool enableOnLevelUp)
    {
        if (enableOnLevelUp)
        {
            isEnabled = true;
        }
        else
        {   
            isDisabledByOutsideSource = false;
        }
        
        if (!IsEnabled() || IsOnCooldown || IsActive || !champion.AbilityUIManager) return;
        
        if (IsBlocked)
        {
            BlockAbility(true);
        }
        else
        {
            champion.AbilityUIManager.EnableAbility(AbilityCategory, ID, resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
        }
    }

    protected void EnableOtherAbility(Ability ability)
    {
        ability.EnableAbility(false);
    }

    public void BlockAbility(bool wasDisabled = false)
    {
        if (!wasDisabled)
        {
            abilityIsBlockedCount++;
            
            if (IsBlocked) return;
            
            IsBlocked = true;
        }
        
        if (!IsOnCooldown && IsEnabled() && champion.AbilityUIManager)
        {
            champion.AbilityUIManager.BlockAbility(AbilityCategory, ID, UsesResource);
        }
    }

    public void UnblockAbility()
    {
        if (!IsBlocked || --abilityIsBlockedCount != 0) return;
        
        IsBlocked = false;
        if (!IsOnCooldown && IsEnabled() && champion.AbilityUIManager)
        {
            champion.AbilityUIManager.UnblockAbility(AbilityCategory, ID, resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
        }
    }

    protected void AbilityHit(Unit unitHit, float damage, bool callOnAbilityHit = true)
    {
        if (damage > 0)
        {
            if (appliesAbilityEffects)
            {
                champion.AbilityEffectsManager.ApplyAbilityEffectsToUnitHit(unitHit, damage);
            }
            if (appliesOnHitEffects)
            {
                champion.OnHitEffectsManager.ApplyOnHitEffectsToUnitHit(unitHit, damage);
            }
        }

        unitHit.EffectSourceManager.UnitHitByAbility(this);
        if (callOnAbilityHit)
        {
            OnAbilityHit?.Invoke(this, unitHit);
        }
    }

    private void StartCooldown(bool calledInStartAbilityCast, bool abilityWasCancelled = false)
    {
        if (!champion.AbilityUIManager) return;
        
        float abilityCooldown = abilityWasCancelled ? cooldownOnCancel : cooldown;
        
        if (calledInStartAbilityCast != startCooldownOnAbilityCast || abilityCooldown <= 0) return;
        
        if (cooldownForRecastCoroutine != null)
        {
            StopCoroutine(cooldownForRecastCoroutine);
        }

        StartCoroutine(PutAbilityOffCooldown(abilityCooldown));
    }

    private void StartCooldownForRecast()
    {
        if (!champion.AbilityUIManager) return;
        
        cooldownForRecastCoroutine = PutAbilityOffCooldownForRecast();
        StartCoroutine(cooldownForRecastCoroutine);
    }

    //This method is used for projectiles that have their origin at the Character's origin. This means you cannot move the origin of the projectile by flashing mid-cast.
    //Examples: EzrealR, MorganaQ, CorkiR
    protected void SetPositionAndRotationOnCast(Vector3 position)
    {
        positionOnCast = position;
        rotationOnCast = Quaternion.LookRotation((destinationOnCast - transform.position).normalized);
    }

    private IEnumerator PutAbilityOffCooldown(float cooldownOnStart)
    {
        IsOnCooldown = true;
        cooldownRemaining = cooldownOnStart;

        yield return null;

        champion.AbilityUIManager.SetAbilityOnCooldown(AbilityCategory, ID, IsBlocked, CanBeRecasted);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            champion.AbilityUIManager.UpdateAbilityCooldown(AbilityCategory, ID, cooldownOnStart, cooldownRemaining);

            yield return null;
        }

        champion.AbilityUIManager.SetAbilityOffCooldown(AbilityCategory, ID, UsesResource, IsEnabled(), IsBlocked);
        if (UsesResource && IsEnabled() && !IsBlocked)
        {
            champion.AbilityUIManager.UpdateAbilityHasEnoughResource(ID, resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
        }

        cooldownForRecastCoroutine = null;
        IsOnCooldown = false;
    }

    private IEnumerator PutAbilityOffCooldownForRecast()
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

            if (UsesResource && resourceCost <= 0)
            {
                UsesResource = false;
                if (abilityUIManager)
                {
                    abilityUIManager.SetAbilityCost(ID, resourceCost);
                }
            }

            if (baseCooldownPerLevel < 0)
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
                abilityUIManager.UpdateAbilityHasEnoughResource(ID,
                    !IsEnabled() || IsBlocked || IsOnCooldown || resourceCost <= champion.StatsManager.Resource.GetCurrentValue());
            }
        }
        else if (AbilityLevel == 0)
        {
            AbilityLevel++;
            EnableAbility(true);
            if (this is IAbilityWithPassive iAbilityWithPassive)
            {
                iAbilityWithPassive.EnableAbilityPassive();
            }
        }
    }

    private void LevelUpAbilityStats()
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
                              bonusADScaling * champion.StatsManager.AttackDamage.GetBonus() +
                              totalADScaling * champion.StatsManager.AttackDamage.GetTotal() +
                              totalAPScaling * champion.StatsManager.AbilityPower.GetTotal();

        return ApplyDamageModifiers(unitHit, abilityDamage, damageType) *
               (isACriticalStrike ? criticalStrikeDamage : 1);
    }

    protected virtual float ApplyAbilityDamageModifier(Unit unitHit)
    {
        return 1;
    }

    protected float ApplyDamageModifiers(Unit unitHit, float damage, DamageType damageType)
    {
        damage *= ApplyAbilityDamageModifier(unitHit);
        switch (damageType)
        {
            case DamageType.MAGIC:
            {
                float totalResistance = unitHit.StatsManager.MagicResistance.GetTotal();
                totalResistance *= (1 - champion.StatsManager.MagicPenetrationPercent.GetTotal());
                totalResistance -= champion.StatsManager.MagicPenetrationFlat.GetTotal();
                return damage * GetResistanceDamageReceivedModifier(totalResistance) * unitHit.StatsManager.MagicDamageReceivedModifier.GetTotal() *
                       champion.StatsManager.MagicDamageModifier.GetTotal();
            }
            case DamageType.PHYSICAL:
            {
                float totalResistance = unitHit.StatsManager.Armor.GetTotal();
                totalResistance *= (1 - champion.StatsManager.ArmorPenetrationPercent.GetTotal());
                totalResistance -= champion.StatsManager.Lethality.GetCurrentValue();
                return damage * GetResistanceDamageReceivedModifier(totalResistance) * unitHit.StatsManager.PhysicalDamageReceivedModifier.GetTotal() *
                       champion.StatsManager.PhysicalDamageModifier.GetTotal();
            }
            case DamageType.TRUE:
                return damage;
            default:
                return damage;
        }
    }

    private float GetResistanceDamageReceivedModifier(float totalResistance)
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

    protected virtual void LevelUpExtraStats() { }

    public virtual void RecastAbility()// ex. ZoeQ will have a different use for this method
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

        FinishAbilityCast(hasReducedCooldownOnAbilityCancel);
    }

    protected virtual void ExtraActionsOnCancel() { }

    public void SetRange(float range)
    {
        this.range = range * StaticObjects.MultiplyingFactor;
    }

    public float GetResourceCost()
    {
        return resourceCost;
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
        else if (delayChargeTime != null)
        {
            abilityEffectCoroutine = AbilityWithChargeTime();
        }
        else
        {
            abilityEffectCoroutine = AbilityWithoutDelay();
        }

        StartCoroutine(abilityEffectCoroutine);
    }

    protected virtual IEnumerator AbilityWithCastTime()
    {
        yield return null;
    }

    protected virtual IEnumerator AbilityWithCastTimeAndChannelTime()
    {
        yield return null;
    }

    protected virtual IEnumerator AbilityWithChannelTime()
    {
        yield return null;
    }

    protected virtual IEnumerator AbilityWithChargeTime()
    {
        yield return null;
    }

    protected virtual IEnumerator AbilityWithoutDelay()
    {
        yield return null;
    }
}
