using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_Q : DirectionTargetedProjectile
{
    private const float DAMAGE_REDUCTION_PER_TARGET_HIT = 0.15f;
    private const float DAMAGE_REDUCTION_CAP = 0.33f;
    private const float DAMAGE_INCREASE_CAP = 0.5f;

    private float rangeIncreaseCap;
    
    private float currentDamageReductionMultiplier;
    private float currentDamageIncreaseMultiplier;
    private float currentRange;
    
    private Varus_W varusW;

    private readonly float cooldownReductionOnVarusWStacksProc;
    private readonly float manaPercentRefundOnChargeEndCancel;

    private readonly WaitForSeconds delayDisableVarusW;
    private readonly WaitForSeconds delayToReduceCooldownWithChargeTime;
    
    protected Varus_Q()
    {
        abilityName = "Piercing Arrow";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 925;
        speed = 1850;
        damage = 10; // 10/46.66f/83.33f/120/156.66f at max charge
        damagePerLevel = 55f * 2f / 3f;
        totalADScaling = 1.1f; // 110%
        resourceCost = 70; // 70/75/80/85/90
        resourceCostPerLevel = 5;
        baseCooldown = 20; // 20/18/16/14/12
        baseCooldownPerLevel = -2;
        chargeTime = 1;
        maximumChargeTime = 4;
        delayChargeTime = new WaitForSeconds(maximumChargeTime);

        rangeIncreaseCap = 700;
        manaPercentRefundOnChargeEndCancel = 0.5f;
        cooldownReductionOnVarusWStacksProc = 4;
        
        delayDisableVarusW = new WaitForSeconds(0.25f);
        delayToReduceCooldownWithChargeTime = new WaitForSeconds(0.15f);
        
        affectedByCooldownReduction = true;

        CanMoveWhileCharging = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusQ";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusQ";
    }
    
    protected override void FinalAdjustments(Vector3 destination)
    {
        currentDamageReductionMultiplier = 1f;
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }
    
    protected override void ModifyValues()
    {
        base.ModifyValues();
        
        rangeIncreaseCap *= StaticObjects.MultiplyingFactor;
    }
    
    protected override void Start()
    {
        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            if (ability is Varus_W varus_W)
            {
                varusW = varus_W;
            }
            else if (ability != this)
            {
                abilitiesToDisableWhileActive.Add(ability);
            }
        }
        
        abilitiesToDisableWhileActive.Add(champion.AbilityManager.OtherCharacterAbilities[0]);
        
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_Q_Buff>() };
    }
    
    public override void UseAbility(Vector3 destination)
    {
        if (IsActive)
        {
            StopCoroutine(abilityEffectCoroutine);

            Buff buff = champion.BuffManager.GetBuff(AbilityBuffs[0]);
            float finalChargeDuration = buff == null ? maximumChargeTime : buff.Duration - buff.DurationRemaining;
            if (finalChargeDuration < chargeTime)
            {
                currentDamageIncreaseMultiplier = 1 + DAMAGE_INCREASE_CAP * finalChargeDuration;
                currentRange = range + rangeIncreaseCap * finalChargeDuration;
            }
            else
            {
                currentDamageIncreaseMultiplier = 1 + DAMAGE_INCREASE_CAP;
                currentRange = range + rangeIncreaseCap;
            }
            
            champion.OrientationManager.RotateCharacterInstantly(destination);     
            SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation);
            
            StartCoroutine(ReduceRemainingCooldownWithChargeDuration(finalChargeDuration));
            AbilityBuffs[0].ConsumeBuff(champion);
            
            FinishAbilityCast();
        }
        else
        {
            StartAbilityCast();

            champion.BasicAttack.CancelCurrentBasicAttackToCastAbility();

            FinalAdjustments(destination);

            StartCorrectCoroutine();
            StartCoroutine(DisableVarusW());
        }
    }
    
    protected override void SpawnProjectile(Vector3 position, Quaternion rotation)
    {
        Projectile projectile = Instantiate(projectilePrefab, position, rotation).GetComponent<Projectile>();
        projectile.ShootProjectile(affectedTeams, affectedUnitTypes, speed, currentRange);
        projectile.OnProjectileHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    private IEnumerator DisableVarusW()
    {
        //this needs to be different if w gets activated and stuff
        yield return delayDisableVarusW;

        //disable varus w
    }
    
    protected override IEnumerator AbilityWithChargeTime()
    {
        UseResource();
        champion.ChampionMovementManager.StopMovementTowardsPointIfHasEvent();
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
        IsBeingCharged = true;

        yield return delayChargeTime;

        IsBeingCharged = false;
        
        champion.StatsManager.Resource.Restore(resourceCost * manaPercentRefundOnChargeEndCancel);
        
        CancelAbility();
    }

    protected override void ExtraActionsOnCancel()
    {
        base.ExtraActionsOnCancel();

        Buff buff = champion.BuffManager.GetBuff(AbilityBuffs[0]);
        float finalChargeDuration = buff == null ? maximumChargeTime : buff.Duration - buff.DurationRemaining;
        StartCoroutine(ReduceRemainingCooldownWithChargeDuration(finalChargeDuration));
    }

    private IEnumerator ReduceRemainingCooldownWithChargeDuration(float finalChargeDuration)
    {        
        yield return delayToReduceCooldownWithChargeTime;
        
        ReduceCooldown(finalChargeDuration);  
    }
    
    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float abilityDamage = GetAbilityDamage(unitHit) * currentDamageReductionMultiplier * currentDamageIncreaseMultiplier;
        Debug.Log(GetAbilityDamage(unitHit) + " " + currentDamageReductionMultiplier + " " + currentDamageIncreaseMultiplier);
        DamageUnit(unitHit, abilityDamage);
        if (currentDamageReductionMultiplier > DAMAGE_REDUCTION_CAP)
        {
            currentDamageReductionMultiplier -= DAMAGE_REDUCTION_PER_TARGET_HIT;
            if (currentDamageReductionMultiplier < DAMAGE_REDUCTION_CAP)
            {
                currentDamageReductionMultiplier = DAMAGE_REDUCTION_CAP;
            }
        }
        if (varusW && varusW.ProcStacks(unitHit, this))
        {
            ReduceCooldown(cooldownReductionOnVarusWStacksProc);
        }

        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }

        AbilityHit(unitHit, abilityDamage);
    }
}
