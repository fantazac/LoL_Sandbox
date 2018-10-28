using UnityEngine;

public abstract class DirectionTargeted : Ability // Curently same as GroundTargeted, might change when other abilities are created
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

        character.EntityBasicAttack.CancelCurrentBasicAttackToCastAbility();

        destinationOnCast = destination;
        RotationOnAbilityCast(destination);

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected virtual void FinalAdjustments(Vector3 destination) { }

    public override bool CanBeCast(Entity target) { return false; }
    public override void UseAbility(Entity target) { }
}
