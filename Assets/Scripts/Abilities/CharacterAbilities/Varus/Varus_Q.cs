using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_Q : DirectionTargetedProjectile
{
    private Varus_W varusW;
    
    private readonly float manaPercentRefundOnChargeEndCancel;

    private readonly WaitForSeconds delayDisabledVarusW;
    private readonly WaitForSeconds delayToReduceCooldownWithChargeTime;
    
    protected Varus_Q()
    {
        abilityName = "Piercing Arrow";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 925;
        speed = 1850;
        damage = 15; // 15/70/125/180/235 at max charge
        damagePerLevel = 55;
        totalADScaling = 1.65f; // 165% at max charge
        resourceCost = 70; // 70/75/80/85/90
        resourceCostPerLevel = 5;
        baseCooldown = 20; // 20/18/16/14/12
        baseCooldownPerLevel = -2;
        chargeTime = 2;
        maximumChargeTime = 4;
        delayChargeTime = new WaitForSeconds(maximumChargeTime);

        manaPercentRefundOnChargeEndCancel = 0.5f;
        
        delayDisabledVarusW = new WaitForSeconds(1);
        delayToReduceCooldownWithChargeTime = new WaitForSeconds(0.1f);
        
        affectedByCooldownReduction = true;

        CanMoveWhileCharging = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusQ";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusQ";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
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
        
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_Q_Buff>() };
    }
    
    public override void UseAbility(Vector3 destination)
    {
        if (IsActive)
        {
            StopCoroutine(abilityEffectCoroutine);
                
            champion.OrientationManager.RotateCharacterInstantly(destination);
                
            SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation);

            StartCoroutine(ReduceRemainingCooldownWithChargeTime());
            
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

    private IEnumerator DisableVarusW()
    {
        //this needs to be different if w gets activated and stuff
        yield return delayDisabledVarusW;

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

        StartCoroutine(ReduceRemainingCooldownWithChargeTime());
    }

    private IEnumerator ReduceRemainingCooldownWithChargeTime()
    {
        Buff buff = champion.BuffManager.GetBuff(AbilityBuffs[0]);

        float finalChargeDuration = buff == null ? maximumChargeTime : buff.Duration - buff.DurationRemaining;
        AbilityBuffs[0].ConsumeBuff(champion);
        
        yield return delayToReduceCooldownWithChargeTime;
        
        ReduceCooldown(finalChargeDuration);  
    }
}
