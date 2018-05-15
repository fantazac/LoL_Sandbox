using UnityEngine;
using System.Collections;

public class CharacterOrientation : MonoBehaviour
{
    private Vector3 lastInstantRotation;

    private Character character;
    private CharacterAbilityManager characterAbilityManager;

    private IEnumerator castPointRotation;
    private IEnumerator movementRotation;
    private IEnumerator targetRotation;
    private bool isRotatingTowardsCastPoint;

    public int RotationSpeed { get; private set; }

    private Vector3 castPointRotationAmountLastFrame;
    private Vector3 castPointRotationAmount;

    private CharacterOrientation()
    {
        RotationSpeed = 18;// note: rotation under 18 makes EzrealE stop rotation for a short time after the ability is done
    }

    private void Awake()
    {
        character = GetComponent<Character>();
        characterAbilityManager = GetComponent<CharacterAbilityManager>();
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

    private IEnumerator Rotate(Vector3 destination)
    {
        while (transform.position != destination)
        {
            if (CanRotate() && character.EntityStatusManager.CanUseMovement())
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

    private IEnumerator RotateUntilReachedTarget(Transform targetTransform, bool isBasicAttack)
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

    private IEnumerator RotateUntilFacingCastPoint(Vector3 castPoint)
    {
        isRotatingTowardsCastPoint = true;

        castPointRotationAmount = Vector3.up;
        castPointRotationAmountLastFrame = Vector3.zero;

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

    private bool CanRotate()
    {
        return !(isRotatingTowardsCastPoint || characterAbilityManager.IsUsingAbilityPreventingRotation() ||
            characterAbilityManager.IsUsingAbilityThatHasACastTime() || characterAbilityManager.IsUsingAbilityThatHasAChannelTime() ||
            characterAbilityManager.IsUsingADashAbility());
    }

    private bool CanRotateTowardsTarget(Transform targetTransform, bool isBasicAttack)
    {
        return CanRotate() && (CanRotateBasicAttack(targetTransform, isBasicAttack) || CanRotateNotBasicAttack(targetTransform, isBasicAttack));
    }

    private bool CanRotateBasicAttack(Transform targetTransform, bool isBasicAttack)
    {
        return isBasicAttack && character.EntityStatusManager.CanUseBasicAttacks() && (character.EntityStatusManager.CanUseMovement() || Vector3.Distance(targetTransform.position, transform.position) <= character.EntityStats.AttackRange.GetTotal());
    }

    private bool CanRotateNotBasicAttack(Transform targetTransform, bool isBasicAttack)
    {
        return !isBasicAttack && character.EntityStatusManager.CanUseBasicAbilities();
    }
}
