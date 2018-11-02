using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tristana_E : UnitTargetedProjectile
{
    private float effectRadius;
    private float effectRadiusOnTurret;

    private float damagePercentIncreasePerStack;
    private List<Ability> abilitiesToIncreaseStacks;

    private float passiveDamage;
    private float passiveDamagePerLevel;
    private float passiveTotalAPScaling;
    private DamageType passiveDamageType;

    protected Tristana_E()
    {
        abilityName = "Explosive Charge";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;//TODO: affect turrets too (see affectedUnitType for array/list)
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;
        passiveDamageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 525;
        speed = 2000;
        damage = 60;// 60/70/80/90/100
        damagePerLevel = 10;
        bonusADScaling = 0.5f;// 50/65/80/95/110%
        bonusADScalingPerLevel = 0.15f;
        totalAPScaling = 0.5f;
        resourceCost = 50;// 50/55/60/65/70
        resourceCostPerLevel = 5;
        baseCooldown = 16;// 16/15.5/15/14.5/14
        baseCooldownPerLevel = -0.5f;
        castTime = 0.15f;//TODO: Check ingame
        delayCastTime = new WaitForSeconds(castTime);

        effectRadius = 300;
        effectRadiusOnTurret = 500;
        damagePercentIncreasePerStack = 0.3f;

        passiveDamage = 55;// 55/80/105/130/155
        passiveDamagePerLevel = 25;
        passiveTotalAPScaling = 0.25f;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaE";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Tristana/TristanaE";
    }

    protected override void ModifyValues()
    {
        effectRadius *= StaticObjects.MultiplyingFactor;
        effectRadiusOnTurret *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Tristana_E_Debuff>(), gameObject.AddComponent<Tristana_E_StackDebuff>() };

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromEntityHit;

        abilitiesToIncreaseStacks = new List<Ability>();
        foreach (Ability ability in character.CharacterAbilityManager.CharacterAbilities)
        {
            if (ability != this && !(ability is PassiveTargeted || ability is SelfTargeted))
            {
                abilitiesToIncreaseStacks.Add(ability);
            }
        }
    }

    public override void EnableAbilityPassive()
    {
        GetComponent<TristanaBasicAttack>().OnBasicAttackKilledEntity += DamageAllEnemiesInPassiveExplosionRadius;
    }

    public override void LevelUpExtraStats()
    {
        passiveDamage += passiveDamagePerLevel;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterOrientationManager.RotateCharacterInstantly(destinationOnCast);

        ProjectileUnitTargeted projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(character.Team, targetedEntity, speed);
        projectile.OnAbilityEffectHit += OnAbilityEffectHit;

        FinishAbilityCast();
    }

    protected override void OnAbilityEffectHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AddNewDebuffToEntityHit(entityHit);
    }

    private void AddNewDebuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityEffectSourceManager.OnEntityHitByAbility += OnMarkedEntityHitByAbility;
        entityHit.EntityEffectSourceManager.OnEntityHitByBasicAttack += OnMarkedEntityHitByBasicAttack;
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }

    private void RemoveDebuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityEffectSourceManager.OnEntityHitByAbility -= OnMarkedEntityHitByAbility;
        entityHit.EntityEffectSourceManager.OnEntityHitByBasicAttack -= OnMarkedEntityHitByBasicAttack;

        Buff stackDebuff = entityHit.EntityBuffManager.GetDebuff(AbilityDebuffs[1]);
        DamageAllEnemiesInActiveExplosionRadius(entityHit, stackDebuff != null ? stackDebuff.CurrentStacks : 0);
        AbilityDebuffs[1].ConsumeBuff(entityHit);
    }

    private void OnMarkedEntityHitByAbility(Entity entityHit, Ability sourceAbility)
    {
        if (entityHit.EntityBuffManager.GetDebuff(AbilityDebuffs[0]) != null && abilitiesToIncreaseStacks.Contains(sourceAbility))
        {
            IncreaseStacksOnEntityHit(entityHit, sourceAbility);
        }
    }

    private void OnMarkedEntityHitByBasicAttack(Entity entityHit, Entity sourceEntity)
    {
        if (entityHit.EntityBuffManager.GetDebuff(AbilityDebuffs[0]) != null && sourceEntity == character)
        {
            IncreaseStacksOnEntityHit(entityHit);
        }
    }

    private void IncreaseStacksOnEntityHit(Entity entityHit, Ability sourceAbility = null)
    {
        AbilityDebuffs[1].AddNewBuffToAffectedEntity(entityHit);
        if (entityHit.EntityBuffManager.GetDebuff(AbilityDebuffs[1]).IsAtMaximumStacks())
        {
            AbilityDebuffs[0].ConsumeBuff(entityHit);
        }
    }

    private void DamageAllEnemiesInActiveExplosionRadius(Entity entityHit, int stacksOnExplosion)
    {
        float damageModifier = stacksOnExplosion * damagePercentIncreasePerStack + 1;

        Entity tempEntity;
        float selectedRadius = effectRadius;//TODO: entityHit is Turret ? effectRadiusOnTurret : effectRadius;

        Vector3 groundPosition = Vector3.right * entityHit.transform.position.x + Vector3.forward * entityHit.transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, selectedRadius))
        {
            tempEntity = collider.GetComponentInParent<Entity>();
            if (tempEntity != null && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
            {
                float damage = GetAbilityDamage(tempEntity) * damageModifier;
                tempEntity.EntityStatsManager.ReduceHealth(damageType, damage);
                AbilityHit(tempEntity, damage);
            }
        }
    }

    private void DamageAllEnemiesInPassiveExplosionRadius(Entity killedEntity)
    {
        Entity tempEntity;

        Vector3 groundPosition = Vector3.right * killedEntity.transform.position.x + Vector3.forward * killedEntity.transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempEntity = collider.GetComponentInParent<Entity>();
            if (tempEntity != null && tempEntity != killedEntity && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
            {
                float damage = GetPassiveAbilityDamage(tempEntity);
                tempEntity.EntityStatsManager.ReduceHealth(passiveDamageType, damage);
                AbilityHit(tempEntity, damage);
            }
        }
    }

    private float GetPassiveAbilityDamage(Entity entityHit)
    {
        float abilityDamage = damage + (passiveTotalAPScaling * character.EntityStatsManager.AbilityPower.GetTotal());

        return ApplyDamageModifiers(entityHit, abilityDamage, passiveDamageType);
    }
}
