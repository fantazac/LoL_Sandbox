using UnityEngine;

public abstract class GroundTargeted : Ability // Currently same as DirectionTargeted, might change when other abilities are created
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    public override Vector3 GetDestination()
    {
        return hit.point + champion.CharacterHeightOffset;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        champion.BasicAttack.CancelCurrentBasicAttackToCastAbility();

        destinationOnCast = destination;
        RotationOnAbilityCast(destination);

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected virtual void FinalAdjustments(Vector3 destination) { }

    public override bool CanBeCast(Unit target) { return false; }
    public override void UseAbility(Unit target) { }
}
