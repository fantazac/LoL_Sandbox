using System.Collections;
using UnityEngine;

public class Tristana_R : UnitTargetedProjectile
{
    private float effectRadius;

    protected Tristana_R()
    {
        abilityName = "Buster Shot";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 3;

        range = 525;
        speed = 2000;
        damage = 300;// 300/400/500
        damagePerLevel = 100;
        totalAPScaling = 1;
        resourceCost = 100;
        baseCooldown = 120;// 120/110/100
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
        character.OrientationManager.RotateCharacterInstantly(destinationOnCast);

        ProjectileUnitTargeted projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(character.Team, targetedUnit, speed);
        projectile.OnAbilityEffectHit += OnAbilityEffectHit;

        AbilityDebuffs[0].SetNormalizedVector(character.transform.position, targetedUnit.transform.position);

        FinishAbilityCast();
    }

    protected override void OnAbilityEffectHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        base.OnAbilityEffectHit(projectile, unitHit, isACriticalStrike, willMiss);

        AddNewDebuffToAllEnemiesInEffectRadius(unitHit);
    }

    private void AddNewDebuffToAllEnemiesInEffectRadius(Unit unitHit)
    {
        Unit tempUnit;

        Vector3 groundPosition = Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempUnit = collider.GetComponentInParent<Unit>();
            if (tempUnit != null && TargetIsValid.CheckIfTargetIsValid(tempUnit, affectedUnitType, character.Team))
            {
                AbilityDebuffs[0].AddNewBuffToAffectedUnit(tempUnit);
            }
        }
    }
}
