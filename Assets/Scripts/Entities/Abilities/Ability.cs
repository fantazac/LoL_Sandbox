using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;

    protected AbilityAffectedUnitType affectedUnitType;//TODO: make this an array/list to support multiple types (ex. Tristana_E targetting enemies and turrets)
    protected AbilityEffectType effectType;
    protected AbilityType abilityType;
    protected DamageType damageType;

    protected IEnumerator abilityEffectCoroutine;
    protected IEnumerator cooldownForRecastCoroutine;

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
    protected Entity targetedEntity;

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

    public Sprite AbilitySprite { get; protected set; }
    protected Sprite abilityRecastSprite;//TODO: Not implemented

    protected string abilitySpritePath;
    protected string abilityRecastSpritePath;

    protected int abilityIsBlockedCount;

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
    public bool OfflineOnly { get; protected set; }
    public bool ResetBasicAttackCycleOnAbilityCast { get; protected set; }
    public bool ResetBasicAttackCycleOnAbilityFinished { get; protected set; }
    public bool UsesResource { get; private set; }

    public List<Ability> AbilitiesToDisableWhileActive { get; protected set; }
    public List<Ability> CastableAbilitiesWhileActive { get; protected set; }

    public AbilityBuff[] AbilityBuffs { get; protected set; }
    public AbilityBuff[] AbilityDebuffs { get; protected set; }

    public delegate void OnAbilityUsedHandler(Ability ability);
    public event OnAbilityUsedHandler OnAbilityUsed;

    public delegate void OnAbilityFinishedHandler(Ability ability);
    public event OnAbilityFinishedHandler OnAbilityFinished;

    public delegate void OnAbilityHitHandler(Ability ability, Entity entityHit);
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
        character = GetComponent<Character>();
        if (character.IsLocalCharacter())
        {
            LoadSprites();
        }
        LoadPrefabs();
    }

    protected virtual void Start()
    {
        HasCastTime = castTime > 0;
        HasChannelTime = channelTime > 0;
        HasReducedCooldownOnAbilityCancel = baseCooldownOnCancel > 0;
        UsesResource = resourceCost > 0;

        if (character.AbilityUIManager && UsesResource)
        {
            character.AbilityUIManager.SetAbilityCost(ID, resourceCost);
        }
        if (affectedByCooldownReduction)
        {
            character.EntityStatsManager.CooldownReduction.OnCooldownReductionChanged += SetCooldownForAbilityAffectedByCooldownReduction;
            SetCooldownForAbilityAffectedByCooldownReduction();
        }
        else
        {
            SetCooldownForAbilityUnaffectedByCooldownReduction();
        }

        ModifyValues();
    }

    public abstract bool CanBeCast(Entity target);
    public abstract bool CanBeCast(Vector3 mousePosition);
    public abstract Vector3 GetDestination();
    public abstract void UseAbility(Entity target);
    public abstract void UseAbility(Vector3 destination);

    protected abstract void SetResourcePaths();

    protected virtual void RotationOnAbilityCast(Vector3 destination)
    {
        character.CharacterOrientationManager.RotateCharacterTowardsCastPoint(destination);
    }

    protected virtual void ModifyValues()
    {
        range *= StaticObjects.MultiplyingFactor;
        speed *= StaticObjects.MultiplyingFactor;
    }

    protected void LoadSprites()
    {
        AbilitySprite = Resources.Load<Sprite>(abilitySpritePath);
        abilityRecastSprite = Resources.Load<Sprite>(abilityRecastSpritePath);
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
        if (character.AbilityTimeBarUIManager)
        {
            if (HasCastTime && HasChannelTime)
            {
                character.AbilityTimeBarUIManager.SetCastTimeAndChannelTime(castTime, channelTime, abilityName, ID);
            }
            else if (HasCastTime)
            {
                character.AbilityTimeBarUIManager.SetCastTime(castTime, abilityName, ID);
            }
            else if (HasChannelTime)
            {
                character.AbilityTimeBarUIManager.SetChannelTime(channelTime, abilityName, ID);
            }
        }
        if (ResetBasicAttackCycleOnAbilityCast)
        {
            character.EntityBasicAttack.ResetBasicAttack();
        }
        StartCooldown(true);
    }

    protected void UseResource()
    {
        if (UsesResource)
        {
            character.EntityStatsManager.Resource.Reduce(resourceCost);
        }
    }

    protected void FinishAbilityCast(bool abilityWasCancelled = false)
    {
        abilityEffectCoroutine = null;
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
            character.EntityBasicAttack.ResetBasicAttack();
        }
        StartCooldown(false, abilityWasCancelled);
    }

    public void DisableAbility()
    {
        IsEnabled = false;
        if (!IsOnCooldown && character.AbilityUIManager)
        {
            character.AbilityUIManager.DisableAbility(AbilityCategory, ID, UsesResource);
        }
    }

    public virtual void EnableAbility()
    {
        if (AbilityLevel > 0)
        {
            IsEnabled = true;
            if (!IsOnCooldown && !IsActive && character.AbilityUIManager)
            {
                if (IsBlocked)
                {
                    BlockAbility();
                }
                else
                {
                    character.AbilityUIManager.EnableAbility(AbilityCategory, ID, resourceCost <= character.EntityStatsManager.Resource.GetCurrentValue());
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
            if (!IsOnCooldown && IsEnabled && character.AbilityUIManager)
            {
                character.AbilityUIManager.BlockAbility(AbilityCategory, ID, UsesResource);
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
                if (!IsOnCooldown && IsEnabled && character.AbilityUIManager)
                {
                    character.AbilityUIManager.UnblockAbility(AbilityCategory, ID, resourceCost <= character.EntityStatsManager.Resource.GetCurrentValue());
                }
            }
        }
    }

    public virtual void EnableAbilityPassive() { }

    protected void AbilityHit(Entity entityHit, float damage, bool callOnAbilityHit = true)
    {
        if (damage > 0)
        {
            if (AppliesAbilityEffects)
            {
                character.CharacterAbilityEffectsManager.ApplyAbilityEffectsToEntityHit(entityHit, damage);
            }
            if (AppliesOnHitEffects)
            {
                character.CharacterOnHitEffectsManager.ApplyOnHitEffectsToEntityHit(entityHit, damage);
            }
        }
        entityHit.EntityEffectSourceManager.EntityHitByAbility(this);
        if (OnAbilityHit != null && callOnAbilityHit)
        {
            OnAbilityHit(this, entityHit);
        }
    }

    private void StartCooldown(bool calledInStartAbilityCast, bool abilityWasCancelled = false)
    {
        if (character.AbilityUIManager)
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
        if (character.AbilityUIManager)
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

        character.AbilityUIManager.SetAbilityOnCooldown(AbilityCategory, ID, IsBlocked);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            character.AbilityUIManager.UpdateAbilityCooldown(AbilityCategory, ID, cooldownOnStart, cooldownRemaining);

            yield return null;
        }

        character.AbilityUIManager.SetAbilityOffCooldown(AbilityCategory, ID, UsesResource, IsEnabled, IsBlocked);
        if (UsesResource && IsEnabled && !IsBlocked)
        {
            character.AbilityUIManager.UpdateAbilityHasEnoughResource(ID, resourceCost <= character.EntityStatsManager.Resource.GetCurrentValue());
        }
        cooldownForRecastCoroutine = null;
        IsOnCooldown = false;
    }

    protected virtual IEnumerator PutAbilityOffCooldownForRecast()
    {
        IsOnCooldownForRecast = true;
        cooldownRemaining = cooldownBeforeRecast;

        yield return null;

        character.AbilityUIManager.SetAbilityOnCooldownForRecast(AbilityCategory, ID);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            character.AbilityUIManager.UpdateAbilityCooldownForRecast(AbilityCategory, ID, cooldownBeforeRecast, cooldownRemaining);

            yield return null;
        }

        character.AbilityUIManager.SetAbilityOffCooldownForRecast(AbilityCategory, ID);
        cooldownForRecastCoroutine = null;
        IsOnCooldownForRecast = false;
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

            AbilityUIManager abilityUIManager = character.AbilityUIManager;

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
                abilityUIManager.UpdateAbilityHasEnoughResource(ID, !IsEnabled || IsBlocked || IsOnCooldown || resourceCost <= character.EntityStatsManager.Resource.GetCurrentValue());
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
        SetCooldownForAbilityAffectedByCooldownReduction(character.EntityStatsManager.CooldownReduction.GetTotal());
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

    protected virtual float GetAbilityDamage(Entity entityHit, bool isACriticalStrike = false, float criticalStrikeDamage = 0)
    {
        float abilityDamage = damage +
            (bonusADScaling * character.EntityStatsManager.AttackDamage.GetBonus()) +
            (totalADScaling * character.EntityStatsManager.AttackDamage.GetTotal()) +
            (totalAPScaling * character.EntityStatsManager.AbilityPower.GetTotal());

        return ApplyDamageModifiers(entityHit, abilityDamage, damageType) *
            (isACriticalStrike ? criticalStrikeDamage : 1f);
    }

    protected virtual float ApplyAbilityDamageModifier(Entity entityHit)
    {
        return 1f;
    }

    protected virtual float ApplyDamageModifiers(Entity entityHit, float damage, DamageType damageType)
    {
        damage *= ApplyAbilityDamageModifier(entityHit);
        if (damageType == DamageType.MAGIC)
        {
            float totalResistance = entityHit.EntityStatsManager.MagicResistance.GetTotal();
            totalResistance *= (1 - character.EntityStatsManager.MagicPenetrationPercent.GetTotal());
            totalResistance -= character.EntityStatsManager.MagicPenetrationFlat.GetTotal();
            return damage * GetResistanceDamageReceivedModifier(totalResistance) * entityHit.EntityStatsManager.MagicDamageReceivedModifier.GetTotal() * character.EntityStatsManager.MagicDamageModifier.GetTotal();
        }
        else if (damageType == DamageType.PHYSICAL)
        {
            float totalResistance = entityHit.EntityStatsManager.Armor.GetTotal();
            totalResistance *= (1 - character.EntityStatsManager.ArmorPenetrationPercent.GetTotal());
            totalResistance -= character.EntityStatsManager.Lethality.GetCurrentValue();
            return damage * GetResistanceDamageReceivedModifier(totalResistance) * entityHit.EntityStatsManager.PhysicalDamageReceivedModifier.GetTotal() * character.EntityStatsManager.PhysicalDamageModifier.GetTotal();
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
            ExtraActionsOnCancel();
            if (character.AbilityTimeBarUIManager && (HasCastTime || HasChannelTime))
            {
                character.AbilityTimeBarUIManager.CancelCastTimeAndChannelTime(ID);
            }
            FinishAbilityCast(HasReducedCooldownOnAbilityCancel);
        }
    }

    protected virtual void ExtraActionsOnCancel() { }

    public virtual void OnEmpoweredBasicAttackHit(Entity entityHit, bool isACriticalStrike = false) { }

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

