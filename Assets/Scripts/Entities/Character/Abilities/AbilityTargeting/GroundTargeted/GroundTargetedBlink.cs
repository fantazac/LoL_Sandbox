using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundTargetedBlink : GroundTargeted
{
    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        destinationOnCast = FindPointToMoveTo(destinationOnCast, transform.position);
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        destinationOnCast = destination;
    }

    protected Vector3 FindPointToMoveTo(Vector3 destination, Vector3 currentPosition)
    {
        float distanceBetweenBothVectors = Vector3.Distance(destination, currentPosition);
        Vector3 normalizedVector = Vector3.Normalize(destination - currentPosition);

        return distanceBetweenBothVectors > range ?
            (range * normalizedVector + currentPosition) : destination;
    }
}
