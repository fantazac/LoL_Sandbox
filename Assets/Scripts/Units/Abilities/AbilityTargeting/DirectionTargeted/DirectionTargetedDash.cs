using UnityEngine;

public abstract class DirectionTargetedDash : DirectionTargeted
{
    protected float minimumDistanceTravelled;
    protected float dashSpeed;
    protected Vector3 destination;

    protected override void ModifyValues()
    {
        minimumDistanceTravelled *= StaticObjects.MultiplyingFactor;
        base.ModifyValues();
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        this.destination = destination;

        character.BasicAttack.CancelCurrentBasicAttackToCastAbility();
        RotationOnAbilityCast(destination);

        if (castTime > 0)
        {
            StartCorrectCoroutine();
        }
        else
        {
            FinalAdjustments(destination);
            UseResource();

            SetupDash();

            FinishAbilityCast();
        }
    }

    protected void SetupDash()
    {
        character.DisplacementManager.SetupDisplacement(this.destination, dashSpeed);
        character.DisplacementManager.OnDisplacementFinished += SetupAutoAttackPostDash;
    }

    protected void SetupAutoAttackPostDash()
    {
        if (!character.MovementManager.IsMovingTowardsPosition())
        {
            character.AutoAttackManager.EnableAutoAttackWithBiggerRange();
        }
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        if (character.AbilityManager.CanRotate())
        {
            character.OrientationManager.RotateCharacterInstantly(destination);
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
            (range * normalizedVector) :
            distanceBetweenBothVectors < minimumDistanceTravelled ?
            (minimumDistanceTravelled * normalizedVector) : distanceBetweenBothVectors * normalizedVector;
    }
}
