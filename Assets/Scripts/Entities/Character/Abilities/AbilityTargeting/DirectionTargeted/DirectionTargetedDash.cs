using UnityEngine;
using System.Collections;

public abstract class DirectionTargetedDash : DirectionTargeted
{
    protected float minimumDistanceTravelled;
    protected float dashSpeed;
    protected Vector3 destination;

    protected override void ModifyValues()
    {
        minimumDistanceTravelled /= StaticObjects.DivisionFactor;
        base.ModifyValues();
    }

    protected override IEnumerator AbilityWithoutCastTime()
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * dashSpeed);

            character.CharacterMovement.NotifyCharacterMoved();

            yield return null;
        }
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
            (range * normalizedVector + currentPosition) :
            distanceBetweenBothVectors < minimumDistanceTravelled ?
            (minimumDistanceTravelled * normalizedVector + currentPosition) : destination;
    }
}
