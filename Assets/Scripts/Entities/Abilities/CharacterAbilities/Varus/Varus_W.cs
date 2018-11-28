using System.Collections;
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
        foreach (Ability ability in character.CharacterAbilityManager.CharacterAbilities)
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

        character.CharacterOnHitEffectsManager.OnApplyOnHitEffects += SetPassiveEffectOnEntityHit;
    }

    public override void LevelUpExtraStats()
    {
        percentHealthDamage += percentHealthDamagePerLevel;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        //if q buff is active
        //AbilityBuffs[1].AddNewBuffToAffectedEntity(character);
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

    private void SetPassiveEffectOnEntityHit(Entity entityHit, float damage)
    {
        AddNewDebuffToEntityHit(entityHit);
        entityHit.EntityStatsManager.ReduceHealth(damageType, GetOnHitDamage(entityHit));
    }

    public void AddNewDebuffToEntityHit(Entity entityHit)
    {
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }

    public void ProcStacks(Entity entityHit, Ability sourceAbility)
    {
        if (entityHit.EntityBuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && abilitiesToTriggerStacks.Contains(sourceAbility))
        {
            DealDamageToEntityWithStacks(entityHit, sourceAbility);
        }
    }

    private void DealDamageToEntityWithStacks(Entity entityHit, Ability sourceAbility)
    {
        float damage = GetStacksTriggeredDamage(entityHit, entityHit.EntityBuffManager.GetDebuff(AbilityDebuffs[0]).CurrentStacks);
        AbilityDebuffs[0].ConsumeBuff(entityHit);
        entityHit.EntityStatsManager.ReduceHealth(damageType, damage);
        AbilityHit(entityHit, damage);
    }

    private float GetOnHitDamage(Entity entityHit)
    {
        float onHitDamage = damage + (totalAPScaling * character.EntityStatsManager.AbilityPower.GetTotal());

        return ApplyDamageModifiers(entityHit, onHitDamage, damageType);
    }

    private float GetStacksTriggeredDamage(Entity entityHit, int stacks)
    {
        float stacksTriggeredDamage = ((percentAPScaling * character.EntityStatsManager.AbilityPower.GetTotal()) + percentHealthDamage) * entityHit.EntityStatsManager.Health.GetTotal() * stacks;
        float damageAfterModifiers = ApplyDamageModifiers(entityHit, stacksTriggeredDamage, damageType);
        //when Monster exists, return entityHit is Monster ? Math.Min(damageAfterModifiers, maxDamageAgainstMonsters) : damageAfterModifiers;
        return damageAfterModifiers;
    }

    private IEnumerator CancelAbilityAfterDelay()
    {
        yield return delayCancelAbility;

        CancelAbility();
    }
}
