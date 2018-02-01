using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundTargeted : Ability // Curently same as DirectionTargeted, might change when other abilities are created
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    public override Vector3 GetDestination()
    {
        return hit.point + character.CharacterMovement.CharacterHeightOffset;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterOrientation.RotateCharacterInstantly(destination);

        FinalAdjustments(destination);

        if (delayCastTime == null)
        {
            StartCoroutine(AbilityWithoutCastTime());
        }
        else
        {
            StartCoroutine(AbilityWithCastTime());
        }
    }

    protected virtual void FinalAdjustments(Vector3 destination) { }

    public override bool CanBeCast(Entity target) { return false; }
    public override void UseAbility(Entity target) { }
}
