using UnityEngine;
using System.Collections;

public class CharacterOrientation : CharacterBase
{
    private Vector3 networkMove;

    [SerializeField]
    private int rotationSpeed = 15;

    private Vector3 rotationAmountLastFrame;
    private Vector3 rotationAmount;

    protected override void Start()
    {
        base.Start();
    }

    public void RotateCharacterInstantly(Vector3 destination)
    {
        transform.rotation = Quaternion.LookRotation((destination - transform.position).normalized);
    }

    public void RotateCharacter(Vector3 destination)
    {
        StopAllCoroutines();
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
        StopAllCoroutines();
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

    private bool CanRotate()
    {
        return !CharacterAbilityManager.IsUsingAbilityPreventingRotation();
    }
}
