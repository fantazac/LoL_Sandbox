using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Ezreal_E : GroundTargetedBlink
{
    private string projectilePrefabPath;
    private GameObject projectilePrefab;

    private float effectRadius;

    protected Ezreal_E()
    {
        abilityName = "Arcane Shift";

        abilityType = AbilityType.BLINK;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 475;
        speed = 1500;
        damage = 80; // 80/130/180/230/280
        damagePerLevel = 50;
        bonusADScaling = 0.5f; // 50%
        totalAPScaling = 0.75f; // 75%
        resourceCost = 90; // 90
        baseCooldown = 25; // 25/22/19/16/13
        baseCooldownPerLevel = -3f;
        castTime = 0.15f; //TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        IsAMovementAbility = true;

        affectedByCooldownReduction = true;

        effectRadius = 600; // Says 750 on wiki, is more like 600 when I tested
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        destinationOnCast = FindGroundPoint(destinationOnCast, transform.position);
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        champion.ChampionMovementManager.NotifyChampionMoved();

        FinishAbilityCast();

        ShootHomingProjectile();
    }

    private void ShootHomingProjectile()
    {
        Unit closestUnit = null;
        float distance = float.MaxValue;

        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            Unit tempUnit = other.GetComponentInParent<Unit>();

            if (!tempUnit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

            if (tempUnit.BuffManager.IsAffectedByDebuff(champion.AbilityManager.CharacterAbilities[1].AbilityDebuffs[0]))
            {
                closestUnit = tempUnit;
                break;
            }

            float tempDistance = Vector3.Distance(transform.position, tempUnit.transform.position);

            if (tempDistance >= distance) continue;

            distance = tempDistance;
            closestUnit = tempUnit;
        }

        if (!closestUnit) return;

        ProjectileUnitTargeted projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(affectedTeams, closestUnit, speed);
        projectile.OnProjectileHit += OnProjectileHit;
    }

    private void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float abilityDamage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, abilityDamage);
        AbilityHit(unitHit, abilityDamage);
        Destroy(projectile.gameObject);
    }
}
