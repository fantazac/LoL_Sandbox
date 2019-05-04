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
    
    protected Vector3 FindGroundPoint(Vector3 destination, Vector3 currentPosition)
    {
        float distanceBetweenBothVectors = Vector3.Distance(destination, currentPosition);
        Vector3 normalizedVector = Vector3.Normalize(destination - currentPosition);

        return distanceBetweenBothVectors > range ? range * normalizedVector + currentPosition : destination;
    }

    protected virtual void FinalAdjustments(Vector3 destination) { }
}
