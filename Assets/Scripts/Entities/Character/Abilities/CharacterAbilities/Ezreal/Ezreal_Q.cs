using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_Q : DirectionTargetedProjectile, CharacterAbility
{
    protected Ezreal_Q()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        range = 1150;
        speed = 2000;
        damage = 100;
        cooldown = 4;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        startCooldownOnAbilityCast = true;

        HasCastTime = true;
    }
}
