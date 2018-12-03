using UnityEngine;
using System.Collections;

public class OrientationManager : MonoBehaviour
{
    protected Vector3 lastInstantRotation;

    protected Unit unit;
    protected AbilityManager characterAbilityManager;

    protected IEnumerator castPointRotation;
    protected IEnumerator movementRotation;
    protected IEnumerator targetRotation;
    protected bool isRotatingTowardsCastPoint;

    public int RotationSpeed { get; private set; }

    protected OrientationManager()
    {
        RotationSpeed = 18;// note: rotation under 18 makes EzrealE stop rotation for a short time after the ability is done
    }

    protected void Awake()
    {
        unit = GetComponent<Unit>();
        characterAbilityManager = GetComponent<AbilityManager>();
    }

    public void RotateCharacterInstantly(Vector3 destination)
    {
        lastInstantRotation = destination;
        Vector3 rotation = (destination - transform.position).normalized;
        if (rotation != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation((destination - transform.position).normalized);
        }
    }

    public void RotateCharacterInstantlyToLastInstantRotation()
    {
        transform.rotation = Quaternion.LookRotation((lastInstantRotation - transform.position).normalized); ;
    }

    public void StopRotation()
    {
        StopMovementRotation();
        StopTargetRotation();
    }

    public void StopMovementRotation()
    {
        if (movementRotation != null)
        {
            StopCoroutine(movementRotation);
            movementRotation = null;
        }
    }

    public void StopTargetRotation()
    {
        if (targetRotation != null)
        {
            StopCoroutine(targetRotation);
            targetRotation = null;
        }
    }

    public void RotateCharacter(Vector3 destination)
    {
        StopRotation();
        movementRotation = Rotate(destination);
        StartCoroutine(movementRotation);
    }

    protected IEnumerator Rotate(Vector3 destination)
    {
        while (transform.position != destination)
        {
            if (CanRotate() && unit.StatusManager.CanUseMovement())
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, destination - transform.position, Time.deltaTime * RotationSpeed, 0));
            }

            yield return null;
        }

        movementRotation = null;
    }

    public void RotateCharacterUntilReachedTarget(Transform targetTransform, bool isBasicAttack, bool stopTargetRotationOnly = false)
    {
        if (stopTargetRotationOnly)
        {
            StopTargetRotation();
        }
        else
        {
            StopRotation();
        }
        targetRotation = RotateUntilReachedTarget(targetTransform, isBasicAttack);
        StartCoroutine(targetRotation);
    }

    protected IEnumerator RotateUntilReachedTarget(Transform targetTransform, bool isBasicAttack)
    {
        while (targetTransform != null)
        {
            if (CanRotateTowardsTarget(targetTransform, isBasicAttack))
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetTransform.position - transform.position, Time.deltaTime * RotationSpeed, 0));
            }

            yield return null;
        }

        targetRotation = null;
    }

    public void RotateCharacterTowardsCastPoint(Vector3 castPoint)
    {
        StopCastPointRotation();
        castPointRotation = RotateUntilFacingCastPoint(castPoint);
        StartCoroutine(castPointRotation);
    }

    public void StopCastPointRotation()
    {
        if (castPointRotation != null)
        {
            StopCoroutine(castPointRotation);
            castPointRotation = null;
            isRotatingTowardsCastPoint = false;
        }
    }

    protected IEnumerator RotateUntilFacingCastPoint(Vector3 castPoint)
    {
        isRotatingTowardsCastPoint = true;

        Vector3 castPointRotationAmount = Vector3.up;
        Vector3 castPointRotationAmountLastFrame = Vector3.zero;

        while (castPointRotationAmountLastFrame != castPointRotationAmount)
        {
            castPointRotationAmountLastFrame = castPointRotationAmount;

            castPointRotationAmount = Vector3.RotateTowards(transform.forward, castPoint - transform.position, Time.deltaTime * RotationSpeed, 0);

            transform.rotation = Quaternion.LookRotation(castPointRotationAmount);

            yield return null;
        }

        castPointRotation = null;
        isRotatingTowardsCastPoint = false;
    }

    protected virtual bool CanRotate()
    {
        return !isRotatingTowardsCastPoint && characterAbilityManager.CanRotate() &&
            !characterAbilityManager.AnAbilityIsBeingCasted() && !characterAbilityManager.AnAbilityInBeingChanneled() &&
            !unit.DisplacementManager.IsBeingDisplaced;
    }

    private bool CanRotateTowardsTarget(Transform targetTransform, bool isBasicAttack)
    {
        return CanRotate() && (CanRotateBasicAttack(targetTransform, isBasicAttack) || CanRotateNotBasicAttack(targetTransform, isBasicAttack));
    }

    private bool CanRotateBasicAttack(Transform targetTransform, bool isBasicAttack)
    {
        return isBasicAttack && unit.StatusManager.CanUseBasicAttacks() && (unit.StatusManager.CanUseMovement() || Vector3.Distance(targetTransform.position, transform.position) <= unit.StatsManager.AttackRange.GetTotal());
    }

    private bool CanRotateNotBasicAttack(Transform targetTransform, bool isBasicAttack)
    {
        return !isBasicAttack && unit.StatusManager.CanUseBasicAbilities();
    }
}
