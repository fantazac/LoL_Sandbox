using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_R : DirectionTargetedProjectile, CharacterAbility
{
    protected Ezreal_R()
    {
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        range = (float)Range.GLOBAL;
        speed = 2000;
        damage = 300;
        castTime = 1;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }
}
