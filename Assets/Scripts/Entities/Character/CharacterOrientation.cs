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
            rotationAmountLastFrame = rotationAmount;

            rotationAmount = Vector3.RotateTowards(transform.forward, destination - transform.position, Time.deltaTime * rotationSpeed, 0);

            transform.rotation = Quaternion.LookRotation(rotationAmount);

            yield return null;
        }
    }
}
