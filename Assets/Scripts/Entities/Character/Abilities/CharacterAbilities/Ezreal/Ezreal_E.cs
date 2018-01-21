using UnityEngine;
using System.Collections;

public class Ezreal_E : GroundTargetedBlink, CharacterAbility
{
    protected Ezreal_E()
    {
        effectType = AbilityEffectType.SINGLE_TARGET;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        range = 475;
        castTime = 0.15f;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();

        //shoot projectile that hits closest target to ezreal on arrival to destination

        FinishAbilityCast();
    }
}
