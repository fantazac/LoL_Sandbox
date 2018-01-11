using UnityEngine;
using System.Collections;

public class Ezreal_E : Blink, CharacterAbility
{
    protected Ezreal_E()
    {
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
