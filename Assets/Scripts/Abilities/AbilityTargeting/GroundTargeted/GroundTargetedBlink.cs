using System.Collections;
using UnityEngine;

public abstract class GroundTargetedBlink : GroundTargeted
{
    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        destinationOnCast = FindGroundPoint(destinationOnCast, transform.position);
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
}
