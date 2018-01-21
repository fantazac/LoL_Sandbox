using UnityEngine;
using System.Collections;

public abstract class GroundTargetedBlink : GroundTargeted
{
    protected Vector3 destination;

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        this.destination = FindPointToMoveTo(destination, transform.position);
    }

    protected Vector3 FindPointToMoveTo(Vector3 destination, Vector3 currentPosition)
    {
        float distanceBetweenBothVectors = Vector3.Distance(destination, currentPosition);
        Vector3 normalizedVector = Vector3.Normalize(destination - currentPosition);

        return distanceBetweenBothVectors > range ?
            (range * normalizedVector + currentPosition) : destination;
    }
}
