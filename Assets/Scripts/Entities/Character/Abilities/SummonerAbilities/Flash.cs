using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : GroundTargetedBlink, SummonerAbility
{
    protected Flash()
    {
        range = 400;
        cooldown = 20;

        startCooldownOnAbilityCast = true;

        CanBeCastAtAnytime = true;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterOrientation.RotateCharacterInstantly(destination);

        transform.position = FindPointToMoveTo(destination, transform.position);
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }
}
