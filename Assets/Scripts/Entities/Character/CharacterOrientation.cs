using UnityEngine;
using System.Collections;

public class CharacterOrientation : MonoBehaviour
{
    private Vector3 lastInstantRotation;

    private CharacterAbilityManager characterAbilityManager;

    private IEnumerator castPointRotation;
    private bool isRotatingTowardsCastPoint;

    private int rotationSpeed = 15;

    private Vector3 rotationAmountLastFrame;
    private Vector3 rotationAmount;

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

    public void StopAllRotation()
    {
        castPointRotation = null;
        isRotatingTowardsCastPoint = false;
        StopAllCoroutines();
    }

    public void RotateCharacter(Vector3 destination)
    {
        StopAllRotation();
        StartCoroutine(Rotate(destination));
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
    }

    public void RotateCharacterUntilReachedTarget(Transform target)
    {
        StopAllRotation();
        StartCoroutine(RotateUntilReachedTarget(target));
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
    }

    public void RotateCharacterTowardsCastPoint(Vector3 castPoint)
    {
        castPointRotation = RotateUntilFacingCastPoint(castPoint);
        StartCoroutine(castPointRotation);
    }

    public void StopRotationTowardsCastPoint()
    {
        StopCoroutine(castPointRotation);
        castPointRotation = null;
        isRotatingTowardsCastPoint = false;
    }

    private IEnumerator RotateUntilFacingCastPoint(Vector3 castPoint)
    {
        isRotatingTowardsCastPoint = true;

        rotationAmount = Vector3.up;
        rotationAmountLastFrame = Vector3.zero;

        while (rotationAmountLastFrame != rotationAmount)
        {
            rotationAmountLastFrame = rotationAmount;

            rotationAmount = Vector3.RotateTowards(transform.forward, castPoint - transform.position, Time.deltaTime * rotationSpeed, 0);

            transform.rotation = Quaternion.LookRotation(rotationAmount);

            yield return null;
        }

        castPointRotation = null;
        isRotatingTowardsCastPoint = false;
    }

    private bool CanRotate()
    {
        return !(isRotatingTowardsCastPoint || characterAbilityManager.IsUsingAbilityPreventingRotation() || 
            characterAbilityManager.IsUsingAbilityThatHasACastTime() || characterAbilityManager.IsUsingADashAbility());
    }
}
