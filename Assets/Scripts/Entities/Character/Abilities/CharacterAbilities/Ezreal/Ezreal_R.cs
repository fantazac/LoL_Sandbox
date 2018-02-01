using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_R : DirectionTargetedProjectile, CharacterAbility
{
    private const float DAMAGE_REDUCTION_PER_TARGET_HIT = 0.1f;
    private const float DAMAGE_REDUCTION_CAP = 0.3f;

    private float currentDamageMultiplier;

    protected Ezreal_R()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.MAGIC;

        range = (float)Range.GLOBAL;
        speed = 2000;
        damage = 300;
        cooldown = 15;
        castTime = 1;
        delayCastTime = new WaitForSeconds(castTime);

        startCooldownOnAbilityCast = true;

        HasCastTime = true;
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        currentDamageMultiplier = 1f;
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.GetComponent<Health>().Reduce(damage * currentDamageMultiplier);
        if(currentDamageMultiplier > DAMAGE_REDUCTION_CAP)
        {
            currentDamageMultiplier -= DAMAGE_REDUCTION_PER_TARGET_HIT;
        }
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
    }
}
