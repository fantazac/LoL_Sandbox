﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_W : PassiveTargeted
{
    private List<Ability> abilitiesToTriggerStacks;

    private IEnumerator cancelAbilityAfterDelayCoroutine;
    private float timeBeforeCancellingAbility;
    private WaitForSeconds delayCancelAbility;

    private float percentHealthDamage;
    private float percentHealthDamagePerLevel;
    private float percentAPScaling;

    //private float maxDamageAgainstMonsters;

    //private float missingHealthDamage;
    //private float missingHealthDamagePerLevel;

    protected Varus_W()
    {
        abilityName = "Blighted Quiver";

        abilityType = AbilityType.PASSIVE;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.MAGIC;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 5;

        damage = 7;// 7/10.5/14/17.5/21
        damagePerLevel = 3.5f;
        baseCooldown = 40;
        baseCooldownOnCancel = 1;
        cooldownBeforeRecast = 1;
        totalAPScaling = 0.25f;

        percentHealthDamage = 0.025f;
        percentHealthDamagePerLevel = 0.005f;
        percentAPScaling = 0.0002f;

        //maxDamageAgainstMonsters = 360;

        //missingHealthDamage = 6;// 6/7/8/9/10
        //missingHealthDamagePerLevel = 1;

        timeBeforeCancellingAbility = 5;
        delayCancelAbility = new WaitForSeconds(timeBeforeCancellingAbility);

        CanBeRecasted = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusW";
        abilityRecastSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusW_Active";
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_W_Debuff>() };

        abilitiesToTriggerStacks = new List<Ability>();
        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            if (ability != this)
            {
                abilitiesToTriggerStacks.Add(ability);
            }
        }
    }

    public override void EnableAbilityPassive()
    {
        LevelUpExtraStats();

        champion.OnHitEffectsManager.OnApplyOnHitEffects += SetPassiveEffectOnUnitHit;
    }

    public override void LevelUpExtraStats()
    {
        percentHealthDamage += percentHealthDamagePerLevel;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        //if q buff is active
        //AbilityBuffs[1].AddNewBuffToAffectedUnit(character);
        //else
        if (cancelAbilityAfterDelayCoroutine != null)
        {
            StopCoroutine(cancelAbilityAfterDelayCoroutine);
        }
        cancelAbilityAfterDelayCoroutine = CancelAbilityAfterDelay();
        StartCoroutine(cancelAbilityAfterDelayCoroutine);
    }

    protected override void ExtraActionsOnCancel()
    {
        if (cancelAbilityAfterDelayCoroutine != null)
        {
            StopCoroutine(cancelAbilityAfterDelayCoroutine);
        }
    }

    private void SetPassiveEffectOnUnitHit(Unit unitHit, float damage)
    {
        AddNewDebuffToAffectedUnit(unitHit);
        DamageUnit(unitHit, GetOnHitDamage(unitHit));
    }

    public void AddNewDebuffToAffectedUnit(Unit affectedUnit)
    {
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(affectedUnit);
    }

    public void ProcStacks(Unit unitHit, Ability sourceAbility)
    {
        if (unitHit.BuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && abilitiesToTriggerStacks.Contains(sourceAbility))
        {
            DealDamageToUnitWithStacks(unitHit, sourceAbility);
        }
    }

    private void DealDamageToUnitWithStacks(Unit unitHit, Ability sourceAbility)
    {
        float damage = GetStacksTriggeredDamage(unitHit, unitHit.BuffManager.GetDebuff(AbilityDebuffs[0]).CurrentStacks);
        AbilityDebuffs[0].ConsumeBuff(unitHit);
        DamageUnit(unitHit, damage);
        AbilityHit(unitHit, damage);
    }

    private float GetOnHitDamage(Unit unitHit)
    {
        float onHitDamage = damage + (totalAPScaling * champion.StatsManager.AbilityPower.GetTotal());

        return ApplyDamageModifiers(unitHit, onHitDamage, damageType);
    }

    private float GetStacksTriggeredDamage(Unit unitHit, int stacks)
    {
        float stacksTriggeredDamage = ((percentAPScaling * champion.StatsManager.AbilityPower.GetTotal()) + percentHealthDamage) * unitHit.StatsManager.Health.GetTotal() * stacks;
        float damageAfterModifiers = ApplyDamageModifiers(unitHit, stacksTriggeredDamage, damageType);
        //when Monster exists, return unitHit is Monster ? Math.Min(damageAfterModifiers, maxDamageAgainstMonsters) : damageAfterModifiers;
        return damageAfterModifiers;
    }

    private IEnumerator CancelAbilityAfterDelay()
    {
        yield return delayCancelAbility;

        CancelAbility();
    }
}
