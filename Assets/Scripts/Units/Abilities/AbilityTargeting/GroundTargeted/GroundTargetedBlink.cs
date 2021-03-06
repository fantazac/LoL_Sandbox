﻿using System.Collections;
using UnityEngine;

public abstract class GroundTargetedBlink : GroundTargeted
{
    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        destinationOnCast = FindPointToMoveTo(destinationOnCast, transform.position);
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        champion.ChampionMovementManager.NotifyChampionMoved();

        FinishAbilityCast();
    }

    protected override IEnumerator AbilityWithChannelTime()
    {
        UseResource();
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        champion.ChampionMovementManager.NotifyChampionMoved();

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
