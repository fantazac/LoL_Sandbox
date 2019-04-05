using UnityEngine;

public abstract class GroundTargeted : Ability, IDestinationTargeted // Currently same as DirectionTargeted, might change when other abilities are created
{
    public virtual bool CanBeCast(Vector3 mousePosition)
    {
        return MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    public virtual Vector3 GetDestination()
    {
        return hit.point + champion.CharacterHeightOffset;
    }

    public virtual void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        champion.BasicAttack.CancelCurrentBasicAttackToCastAbility();

        destinationOnCast = destination;
        RotationOnAbilityCast(destination);

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected virtual void FinalAdjustments(Vector3 destination) { }
}
