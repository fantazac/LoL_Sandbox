using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DirectionTargeted : Ability
{
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

        if (delayCastTime == null)
        {
            StartCoroutine(AbilityWithoutCastTime());
        }
        else
        {
            StartCoroutine(AbilityWithCastTime());
        }
    }
}
