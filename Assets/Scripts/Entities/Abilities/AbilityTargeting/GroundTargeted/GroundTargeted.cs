﻿using UnityEngine;

public abstract class GroundTargeted : Ability // Curently same as DirectionTargeted, might change when other abilities are created
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    public override Vector3 GetDestination()
    {
        return hit.point + character.CharacterMovementManager.CharacterHeightOffset;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.BasicAttackManager.CancelCurrentBasicAttackToCastAbility();

        destinationOnCast = destination;
        RotationOnAbilityCast(destination);

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected virtual void FinalAdjustments(Vector3 destination) { }

    public override bool CanBeCast(Entity target) { return false; }
    public override void UseAbility(Entity target) { }
}
