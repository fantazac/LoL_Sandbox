using UnityEngine;
using System.Collections;

public abstract class DirectionTargetedDash : DirectionTargeted
{
    protected float minimumDistanceTravelled;
    protected float dashSpeed;
    protected Vector3 destination;

    protected IEnumerator currentDashCoroutine;

    protected DirectionTargetedDash()
    {
        IsADash = true;
        currentDashCoroutine = null;
    }

    protected override void ModifyValues()
    {
        minimumDistanceTravelled /= StaticObjects.DivisionFactor;
        base.ModifyValues();
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        RotationOnAbilityCast(destination);

        FinalAdjustments(destination);

        if (delayCastTime == null)
        {
            currentDashCoroutine = AbilityWithoutCastTime();
        }
        else
        {
            currentDashCoroutine = AbilityWithCastTime();
        }
        StartCoroutine(currentDashCoroutine);
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        if (!character.CharacterAbilityManager.IsUsingAbilityPreventingRotation())
        {
            character.CharacterOrientation.RotateCharacterInstantly(destination);
        }
    }

    protected override IEnumerator AbilityWithoutCastTime()
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * dashSpeed);

            character.CharacterMovement.NotifyCharacterMoved();

            yield return null;
        }
        currentDashCoroutine = null;
        FinishAbilityCast();
    }

    public void StopDash()
    {
        if (currentDashCoroutine != null)
        {
            StopCoroutine(currentDashCoroutine);
            currentDashCoroutine = null;
            FinishAbilityCast();
        }
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
