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

        abilityType = AbilityType.Skillshot;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 650;//TODO: Check ingame, apparently becomes bigger when attackrange increases?
        speed = 1800;//TODO: Check ingame
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

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneQ";

        projectilePrefabPath = "CharacterAbilities/MissFortune/MissFortuneQ";
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
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);

        ProjectileUnitTargeted projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(character.Team, targetedEntity, speed);
        projectile.OnAbilityEffectHit += OnAbilityEffectHit;
        FinishAbilityCast();
    }

    protected override void OnAbilityEffectHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit();
    }
}
