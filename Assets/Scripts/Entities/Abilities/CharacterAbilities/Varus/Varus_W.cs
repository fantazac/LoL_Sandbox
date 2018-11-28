using System.Collections.Generic;

public class Varus_W : PassiveTargeted
{
    private List<Ability> abilitiesToTriggerStacks;

    private float percentHealthDamage;
    private float percentHealthDamagePerLevel;
    private float percentAPScaling;

    private float maxDamageAgainstMonsters;

    private float missingHealthDamage;
    private float missingHealthDamagePerLevel;

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

        maxDamageAgainstMonsters = 360;

        missingHealthDamage = 6;// 6/7/8/9/10
        missingHealthDamagePerLevel = 1;

        CanBeRecasted = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusW";
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

        return ApplyDamageModifiers(entityHit, stacksTriggeredDamage, damageType);
    }
}
