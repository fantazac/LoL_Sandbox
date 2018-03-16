using UnityEngine;
using System.Collections;

public class CharacterOrientation : MonoBehaviour
{
    private Vector3 lastInstantRotation;

    private CharacterAbilityManager characterAbilityManager;

    private IEnumerator castPointRotation;
    private IEnumerator movementRotation;
    private bool isRotatingTowardsCastPoint;

    private int rotationSpeed = 15;

    private Vector3 rotationAmountLastFrame;
    private Vector3 rotationAmount;

    private Vector3 castPointRotationAmountLastFrame;
    private Vector3 castPointRotationAmount;

    private void Awake()
    {
        characterAbilityManager = GetComponent<CharacterAbilityManager>();
    }

    public void RotateCharacterInstantly(Vector3 destination)
    {
        lastInstantRotation = destination;
        transform.rotation = Quaternion.LookRotation((destination - transform.position).normalized);
    }

    public void RotateCharacterInstantlyToLastInstantRotation()
    {
        transform.rotation = Quaternion.LookRotation((lastInstantRotation - transform.position).normalized); ;
    }

    public void StopMovementRotation()
    {
        if(movementRotation != null)
        {
            StopCoroutine(movementRotation);
            movementRotation = null;
        }
    }

    public void RotateCharacter(Vector3 destination)
    {
        StopMovementRotation();
        movementRotation = Rotate(destination);
        StartCoroutine(movementRotation);
    }

    private IEnumerator Rotate(Vector3 destination)
    {
        rotationAmount = Vector3.up;
        rotationAmountLastFrame = Vector3.zero;

        while (rotationAmountLastFrame != rotationAmount)
        {
            if (CanRotate())
            {
                rotationAmountLastFrame = rotationAmount;

                rotationAmount = Vector3.RotateTowards(transform.forward, destination - transform.position, Time.deltaTime * rotationSpeed, 0);

                transform.rotation = Quaternion.LookRotation(rotationAmount);
            }

            yield return null;
        }

        movementRotation = null;
    }

    public void RotateCharacterUntilReachedTarget(Transform target)
    {
        StopMovementRotation();
        movementRotation = RotateUntilReachedTarget(target);
        StartCoroutine(movementRotation);
    }

    private IEnumerator RotateUntilReachedTarget(Transform target)
    {
        rotationAmount = Vector3.up;
        rotationAmountLastFrame = Vector3.zero;

        while (target != null)
        {
            if (CanRotate())
            {
                rotationAmountLastFrame = rotationAmount;

                rotationAmount = Vector3.RotateTowards(transform.forward, target.position - transform.position, Time.deltaTime * rotationSpeed, 0);

                transform.rotation = Quaternion.LookRotation(rotationAmount);
            }

            yield return null;
        }

        movementRotation = null;
    }

    public void RotateCharacterTowardsCastPoint(Vector3 castPoint)
    {
        StopRotationTowardsCastPoint();
        castPointRotation = RotateUntilFacingCastPoint(castPoint);
        StartCoroutine(castPointRotation);
    }

    public void StopRotationTowardsCastPoint()
    {
        if(castPointRotation != null)
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

            castPointRotationAmount = Vector3.RotateTowards(transform.forward, castPoint - transform.position, Time.deltaTime * rotationSpeed, 0);

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
}
