﻿using UnityEngine;
using System.Collections;

public abstract class Blink : Ability
{
    protected float range;
    protected float minimumDistanceTravelled;
    protected Vector3 destination;

    protected override void ModifyValues()
    {
        range /= StaticObjects.DivisionFactor;
        minimumDistanceTravelled /= StaticObjects.DivisionFactor;
    }

    public override bool CanBeCast(Vector3 mousePosition)
    {
        return !character.CharacterAbilityManager.IsUsingAbilityPreventingAbilityCasts() && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
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

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();

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
