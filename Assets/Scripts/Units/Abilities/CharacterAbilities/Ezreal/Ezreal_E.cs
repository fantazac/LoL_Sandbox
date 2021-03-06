﻿using UnityEngine;
using System.Collections;

public class Ezreal_E : GroundTargetedBlink
{
    private string projectilePrefabPath;
    private GameObject projectilePrefab;

    private float effectRadius;

    protected Ezreal_E()
    {
        abilityName = "Arcane Shift";

        abilityType = AbilityType.BLINK;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 475;
        speed = 1500;
        damage = 80;// 80/130/180/230/280
        damagePerLevel = 50;
        bonusADScaling = 0.5f;// 50%
        totalAPScaling = 0.75f;// 75%
        resourceCost = 90;// 90
        baseCooldown = 19;// 19/17.5f/16/14.5f/13
        baseCooldownPerLevel = -1.5f;
        castTime = 0.15f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        IsAMovementAbility = true;

        affectedByCooldownReduction = true;

        effectRadius = 600;// Says 750 on wiki, is more like 600 when I tested
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealE";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealE";
    }

    protected override void ModifyValues()
    {
        effectRadius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        destinationOnCast = FindPointToMoveTo(destinationOnCast, transform.position);
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        champion.ChampionMovementManager.NotifyChampionMoved();

        FinishAbilityCast();

        ShootHomingProjectile();
    }

    private void ShootHomingProjectile()
    {
        Unit closestUnit = null;
        Unit tempUnit;
        float distance = float.MaxValue;
        float tempDistance;

        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempUnit = collider.GetComponentInParent<Unit>();
            if (tempUnit != null && TargetIsValid.CheckIfTargetIsValid(tempUnit, affectedUnitType, champion.Team))
            {
                if (tempUnit.BuffManager.IsAffectedByDebuff(champion.AbilityManager.CharacterAbilities[1].AbilityDebuffs[0]))
                {
                    closestUnit = tempUnit;
                    break;
                }
                tempDistance = Vector3.Distance(transform.position, tempUnit.transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    closestUnit = tempUnit;
                }
            }
        }

        if (closestUnit != null)
        {
            ProjectileUnitTargeted projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<ProjectileUnitTargeted>();
            projectile.ShootProjectile(champion.Team, closestUnit, speed);
            projectile.OnAbilityEffectHit += OnProjectileHit;
        }
    }

    private void OnProjectileHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, damage);
        AbilityHit(unitHit, damage);
        Destroy(projectile.gameObject);
    }
}
