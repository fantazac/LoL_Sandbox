using UnityEngine;
using System.Collections;

public abstract class Dash : Ability
{
    protected float minimumDistanceTravelled;
    protected float dashSpeed;
    protected Vector3 destination;

    protected override void ModifyValues()
    {
        minimumDistanceTravelled /= StaticObjects.DivisionFactor;
        base.ModifyValues();
    }

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return !characterAbilityManager.IsUsingAbilityPreventingAbilityCasts() && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    public override Vector3 GetDestination()
    {
        return hit.point + character.CharacterMovement.CharacterHeightOffset;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovement.StopAllMovement(this);
        character.CharacterOrientation.RotateCharacterInstantly(destination);

        this.destination = FindPointToMoveTo(destination, transform.position);

        if (delayCastTime == null)
        {
            StartCoroutine(AbilityWithoutCastTime());
        }
        else
        {
            StartCoroutine(AbilityWithCastTime());
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

        FinishAbilityCast();
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
