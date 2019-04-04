using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_Q : UnitTargetedProjectile
{
    private float effectRadius;
    private float effectRadiusOnBigAngle;

    private Vector3 vectorOnCast;

    protected MissFortune_Q()
    {
        abilityName = "Double Up";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 650; //TODO: Check in game, apparently becomes bigger when attack range increases?
        speed = 1700; //TODO: Check in game
        damage = 20; // 20/40/60/80/100
        damagePerLevel = 20;
        totalADScaling = 1; // 100%
        totalAPScaling = 0.35f; // 35%
        resourceCost = 43; // 43/46/49/52/55
        resourceCostPerLevel = 3;
        baseCooldown = 7; // 7/6/5/4/3
        baseCooldownPerLevel = -1;
        castTime = 0.25f; //TODO: Check in game
        delayCastTime = new WaitForSeconds(castTime);

        effectRadius = 500;
        effectRadiusOnBigAngle = 150;

        AppliesOnHitEffects = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneQ";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/MissFortune/MissFortuneQ";
    }

    protected override void ModifyValues()
    {
        effectRadius *= StaticObjects.MultiplyingFactor;
        effectRadiusOnBigAngle *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        vectorOnCast = targetedUnit.transform.position - transform.position;
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
        base.OnProjectileHit(projectile, unitHit, isACriticalStrike, willMiss);
        Unit nextUnit = FindTargetBehindUnitHit(unitHit);

        if (!nextUnit) return;

        bool secondHitIsACriticalStrike =
            unitHit.StatsManager.Health.IsDead() || AttackIsCritical.CheckIfAttackIsCritical(champion.StatsManager.CriticalStrikeChance.GetTotal());

        ProjectileUnitTargeted projectile2 = (Instantiate(projectilePrefab, unitHit.transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile2.transform.LookAt(nextUnit.transform.position);
        projectile2.ShootProjectile(affectedTeams, nextUnit, speed, secondHitIsACriticalStrike);
        projectile2.OnProjectileHit += base.OnProjectileHit;
    }

    private Unit FindTargetBehindUnitHit(Unit unitHit)
    {
        Unit closestEnemy = null;
        float enemyAngle = float.MaxValue;
        float enemyDistance = float.MaxValue;

        Vector3 groundPosition = Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            Unit tempUnit = other.GetComponentInParent<Unit>();

            if (!tempUnit || tempUnit == unitHit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

            float tempAngle = Vector3.Angle(vectorOnCast, tempUnit.transform.position - unitHit.transform.position);
            float tempDistance = Vector3.Distance(unitHit.transform.position, tempUnit.transform.position);
            if ((tempAngle <= 10 && tempDistance < enemyDistance) ||
                (enemyAngle > 10 && tempAngle <= 20 && tempDistance < enemyDistance) ||
                (enemyAngle > 20 && tempAngle <= 55 && tempDistance < enemyDistance) ||
                (enemyAngle > 55 && tempAngle <= 80 && tempDistance <= effectRadiusOnBigAngle && tempDistance < enemyDistance))
            {
                closestEnemy = tempUnit;
                enemyAngle = tempAngle;
                enemyDistance = tempDistance;
            }
        }

        return closestEnemy;
    }
}
