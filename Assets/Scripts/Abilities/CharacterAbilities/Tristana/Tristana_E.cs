using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tristana_E : UnitTargetedProjectile
{
    private float effectRadius;
    private float effectRadiusOnTurret;

    private readonly float damagePercentIncreasePerStack;
    private List<Ability> abilitiesToIncreaseStacks;

    private float passiveDamage;
    private readonly float passiveDamagePerLevel;
    private readonly float passiveTotalAPScaling;
    private readonly DamageType passiveDamageType;

    protected Tristana_E()
    {
        abilityName = "Explosive Charge";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) }; // typeof(Turret)
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;
        passiveDamageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 525;
        speed = 2000;
        damage = 70; // 70/80/90/100/110
        damagePerLevel = 10;
        bonusADScaling = 0.5f; // 50/70/90/110/130%
        bonusADScalingPerLevel = 0.2f;
        totalAPScaling = 0.5f;
        resourceCost = 50; // 50/55/60/65/70
        resourceCostPerLevel = 5;
        baseCooldown = 16; // 16/15.5/15/14.5/14
        baseCooldownPerLevel = -0.5f;
        castTime = 0.15f; //TODO: Check ingame
        delayCastTime = new WaitForSeconds(castTime);

        effectRadius = 300;
        effectRadiusOnTurret = 500;
        damagePercentIncreasePerStack = 0.3f;

        passiveDamage = 55; // 55/80/105/130/155
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Tristana_E_Debuff>(), gameObject.AddComponent<Tristana_E_StackDebuff>() };

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromAffectedUnit;

        abilitiesToIncreaseStacks = new List<Ability>();
        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            if (ability != this && !(ability is PassiveTargeted || ability is AutoTargeted))
            {
                abilitiesToIncreaseStacks.Add(ability);
            }
        }

        champion.BasicAttack.OnKilledUnit += DamageAllEnemiesInPassiveExplosionRadius;
    }

    protected override void LevelUpExtraStats()
    {
        passiveDamage += passiveDamagePerLevel;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);

        ProjectileUnitTargeted projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(affectedTeams, targetedUnit, speed);
        projectile.OnProjectileHit += OnProjectileHit;

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }

        AddNewDebuffToAffectedUnit(unitHit);
    }

    private void AddNewDebuffToAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.EffectSourceManager.OnUnitHitByAbility += OnMarkedUnitHitByAbility;
        affectedUnit.EffectSourceManager.OnUnitHitByBasicAttack += OnMarkedUnitHitByBasicAttack;
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(affectedUnit);
    }

    private void RemoveDebuffFromAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.EffectSourceManager.OnUnitHitByAbility -= OnMarkedUnitHitByAbility;
        affectedUnit.EffectSourceManager.OnUnitHitByBasicAttack -= OnMarkedUnitHitByBasicAttack;

        if (affectedUnit.BuffManager.IsAffectedByDebuff(AbilityDebuffs[1]))
        {
            DamageAllEnemiesInActiveExplosionRadius(affectedUnit, affectedUnit.BuffManager.GetDebuff(AbilityDebuffs[1]).CurrentStacks);
        }
        else
        {
            DamageAllEnemiesInActiveExplosionRadius(affectedUnit);
        }

        AbilityDebuffs[1].ConsumeBuff(affectedUnit);
    }

    private void OnMarkedUnitHitByAbility(Unit unitHit, Ability sourceAbility)
    {
        if (unitHit.BuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && abilitiesToIncreaseStacks.Contains(sourceAbility))
        {
            IncreaseStacksOnUnitHit(unitHit, sourceAbility);
        }
    }

    private void OnMarkedUnitHitByBasicAttack(Unit unitHit, Unit sourceUnit)
    {
        if (unitHit.BuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && sourceUnit == champion)
        {
            IncreaseStacksOnUnitHit(unitHit);
        }
    }

    private void IncreaseStacksOnUnitHit(Unit unitHit, Ability sourceAbility = null)
    {
        AbilityDebuffs[1].AddNewBuffToAffectedUnit(unitHit);
        if (unitHit.BuffManager.GetDebuff(AbilityDebuffs[1]).IsAtMaximumStacks())
        {
            AbilityDebuffs[0].ConsumeBuff(unitHit);
        }
    }

    private void DamageAllEnemiesInActiveExplosionRadius(Unit unitHit, int stacksOnExplosion = 0)
    {
        float damageModifier = stacksOnExplosion * damagePercentIncreasePerStack + 1;

        float selectedRadius = effectRadius; //TODO: unitHit is Turret ? effectRadiusOnTurret : effectRadius;

        Vector3 groundPosition = Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, selectedRadius))
        {
            Unit tempUnit = other.GetComponentInParent<Unit>();

            if (!tempUnit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

            float abilityDamage = GetAbilityDamage(tempUnit) * damageModifier;
            DamageUnit(tempUnit, abilityDamage);
            AbilityHit(tempUnit, abilityDamage);
        }
    }

    private void DamageAllEnemiesInPassiveExplosionRadius(DamageSource damageSource, Unit killedUnit)
    {
        Vector3 groundPosition = Vector3.right * killedUnit.transform.position.x + Vector3.forward * killedUnit.transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            Unit tempUnit = other.GetComponentInParent<Unit>();

            if (!tempUnit || tempUnit == killedUnit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

            float passiveAbilityDamage = GetPassiveAbilityDamage(tempUnit);
            DamageUnit(tempUnit, passiveDamageType, passiveAbilityDamage);
            AbilityHit(tempUnit, passiveAbilityDamage);
        }
    }

    private float GetPassiveAbilityDamage(Unit unitHit)
    {
        float abilityDamage = passiveDamage + passiveTotalAPScaling * champion.StatsManager.AbilityPower.GetTotal();

        return ApplyDamageModifiers(unitHit, abilityDamage, passiveDamageType);
    }
}
