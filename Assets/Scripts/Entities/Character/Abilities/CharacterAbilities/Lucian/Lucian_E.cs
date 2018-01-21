using UnityEngine;
using System.Collections;

public class Lucian_E : DirectionTargetedDash, CharacterAbility
{
    protected Lucian_E()
    {
        range = 425;
        minimumDistanceTravelled = 100;
        dashSpeed = 32;

        CanCastOtherAbilitiesWithCasting = true;
        CanStopMovement = true;
    }
}
