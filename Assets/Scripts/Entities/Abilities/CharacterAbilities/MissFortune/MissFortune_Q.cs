﻿using System.Collections;
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
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 650;//TODO: Check ingame, apparently becomes bigger when attackrange increases?
        speed = 1700;//TODO: Check ingame
        damage = 20;// 20/40/60/80/100
        damagePerLevel = 20;
        totalADScaling = 1;// 100%
        totalAPScaling = 0.35f;// 35%
        resourceCost = 43;// 43/46/49/52/55
        resourceCostPerLevel = 3;
        baseCooldown = 7;// 7/6/5/4/3
        baseCooldownPerLevel = -1;
        castTime = 0.25f;//TODO: Check ingame
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

    protected override IEnumerator AbilityWithCastTime()
    {
        vectorOnCast = targetedEntity.transform.position - transform.position;
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
        base.OnAbilityEffectHit(projectile, entityHit, isACriticalStrike, willMiss);
        Entity nextEntity = FindTargetBehindEntityHit(entityHit);
        if (nextEntity)
        {
            bool secondHitIsACriticalStrike;
            if (entityHit.EntityStats.Health.IsDead())
            {
                secondHitIsACriticalStrike = true;
            }
            else
            {
                secondHitIsACriticalStrike = AttackIsCritical.CheckIfAttackIsCritical(character.EntityStats.CriticalStrikeChance.GetTotal());
            }

            ProjectileUnitTargeted projectile2 = (Instantiate(projectilePrefab, entityHit.transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
            projectile2.transform.LookAt(nextEntity.transform.position);
            projectile2.ShootProjectile(character.Team, nextEntity, speed, secondHitIsACriticalStrike);
            projectile2.OnAbilityEffectHit += base.OnAbilityEffectHit;
        }
    }

    private Entity FindTargetBehindEntityHit(Entity entityHit)
    {
        Entity closestEnemy = null;
        float enemyAngle = float.MaxValue;
        float enemyDistance = float.MaxValue;
        Entity tempEntity;
        float tempAngle;
        float tempDistance;

        Vector3 groundPosition = Vector3.right * entityHit.transform.position.x + Vector3.forward * entityHit.transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempEntity = collider.GetComponentInParent<Entity>();
            if (tempEntity != null && tempEntity != entityHit && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
            {
                tempAngle = Vector3.Angle(vectorOnCast, tempEntity.transform.position - entityHit.transform.position);
                tempDistance = Vector3.Distance(entityHit.transform.position, tempEntity.transform.position);
                if ((tempAngle <= 10 && tempDistance < enemyDistance) ||
                    (enemyAngle > 10 && tempAngle <= 20 && tempDistance < enemyDistance) ||
                    (enemyAngle > 20 && tempAngle <= 55 && tempDistance < enemyDistance) ||
                    (enemyAngle > 55 && tempAngle <= 80 && tempDistance <= effectRadiusOnBigAngle && tempDistance < enemyDistance))
                {
                    closestEnemy = tempEntity;
                    enemyAngle = tempAngle;
                    enemyDistance = tempDistance;
                }
            }
        }

        return closestEnemy;
    }
}
