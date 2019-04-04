using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tristana_R : UnitTargetedProjectile
{
    private float effectRadius;

    protected Tristana_R()
    {
        abilityName = "Buster Shot";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 3;

        range = 525;
        speed = 2000;
        damage = 300; // 300/400/500
        damagePerLevel = 100;
        totalAPScaling = 1;
        resourceCost = 100;
        baseCooldown = 120; // 120/110/100
        baseCooldownPerLevel = -10;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        effectRadius = 200;

        IsAnUltimateAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaR";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Tristana/TristanaR";
    }

    protected override void ModifyValues()
    {
        effectRadius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Tristana_R_Debuff>() };
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

        AbilityDebuffs[0].SetNormalizedVector(champion.transform.position, targetedUnit.transform.position);

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        base.OnProjectileHit(projectile, unitHit, isACriticalStrike, willMiss);

        AddNewDebuffToAllEnemiesInEffectRadius(unitHit);
    }

    private void AddNewDebuffToAllEnemiesInEffectRadius(Unit unitHit)
    {
        Vector3 groundPosition = Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            Unit tempUnit = other.GetComponentInParent<Unit>();
            if (tempUnit && tempUnit.IsTargetable(affectedUnitTypes, affectedTeams))
            {
                AbilityDebuffs[0].AddNewBuffToAffectedUnit(tempUnit);
            }
        }
    }
}
