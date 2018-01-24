using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile, CharacterAbility
{
    protected Ezreal_W()
    {
        affectedUnitType = AbilityAffectedUnitType.CHARACTERS;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.MAGIC;

        range = 1000;
        speed = 1550;
        damage = 80;
        castTime = 0.2f;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected void ApplyBuffToTarget(Entity entityHit)
    {
        Debug.Log("Increased " + entityHit.gameObject + "'s Attack Speed");
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        if (entityHit.Team == character.Team)
        {
            ApplyBuffToTarget(entityHit);
        }
        else
        {
            entityHit.GetComponent<Health>().Hit(damage);
        }
    }
}
