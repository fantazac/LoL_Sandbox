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

        champion.BasicAttack.CancelCurrentBasicAttackToCastAbility();
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
        champion.DisplacementManager.SetupDisplacement(destination, dashSpeed);
        champion.DisplacementManager.OnDisplacementFinished += SetupAutoAttackPostDash;
    }

    private void SetupAutoAttackPostDash()
    {
        if (!champion.ChampionMovementManager.IsMovingTowardsPosition())
        {
            champion.AutoAttackManager.EnableAutoAttackWithBiggerRange();
        }
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        if (champion.AbilityManager.CanRotate())
        {
            champion.OrientationManager.RotateCharacterInstantly(destination);
        }
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        this.destination = FindPointToMoveTo(destination, transform.position);
    }

    private Vector3 FindPointToMoveTo(Vector3 destination, Vector3 currentPosition)
    {
        float distanceBetweenBothVectors = Vector3.Distance(destination, currentPosition);
        Vector3 normalizedVector = Vector3.Normalize(destination - currentPosition);

        return distanceBetweenBothVectors > range ? (range * normalizedVector) :
            distanceBetweenBothVectors < minimumDistanceTravelled ? (minimumDistanceTravelled * normalizedVector) : distanceBetweenBothVectors * normalizedVector;
    }
}
