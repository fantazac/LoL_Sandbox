using System.Collections;
using UnityEngine;

public class DisplacementManager : MonoBehaviour
{
    private Character unit;//TODO: Unit

    private IEnumerator currentDisplacementCoroutine;
    private AbilityBuff sourceAbilityBuffForCurrentDisplacement;

    public bool IsBeingDisplaced { get { return currentDisplacementCoroutine != null; } }

    public delegate void OnDisplacementFinishedHandler();
    public event OnDisplacementFinishedHandler OnDisplacementFinished;

    private void Start()
    {
        unit = GetComponent<Character>();
    }

    public void SetupDisplacement(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff = null, bool isAKnockup = false)
    {
        StopCurrentDisplacement();

        destination += transform.position;
        currentDisplacementCoroutine = isAKnockup ? Knockup(destination, displacementSpeed, sourceAbilityBuff) : Displacement(destination, displacementSpeed, sourceAbilityBuff);
        sourceAbilityBuffForCurrentDisplacement = sourceAbilityBuff;
        StartCoroutine(currentDisplacementCoroutine);
    }

    public void StopCurrentDisplacement()
    {
        OnDisplacementFinished = null;
        if (currentDisplacementCoroutine != null)
        {
            StopCoroutine(currentDisplacementCoroutine);
            currentDisplacementCoroutine = null;
            if (sourceAbilityBuffForCurrentDisplacement)
            {
                sourceAbilityBuffForCurrentDisplacement.ConsumeBuff(unit);
                sourceAbilityBuffForCurrentDisplacement = null;
            }
            unit.ModelObject.transform.position = transform.position;
        }
    }

    private IEnumerator Displacement(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * displacementSpeed);

            if (unit.CharacterMovementManager)
            {
                unit.CharacterMovementManager.NotifyCharacterMoved();
            }

            yield return null;
        }

        if (OnDisplacementFinished != null)
        {
            OnDisplacementFinished();
        }
        StopCurrentDisplacement();
    }

    private IEnumerator Knockup(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff)
    {
        Vector3 initialPosition = transform.position;
        Transform modelTransform = unit.ModelObject.transform;

        while (modelTransform.position != destination)//up
        {
            modelTransform.position = Vector3.MoveTowards(modelTransform.position, destination, Time.deltaTime * displacementSpeed);

            yield return null;
        }

        destination = initialPosition;

        while (modelTransform.position != destination)//down
        {
            modelTransform.position = Vector3.MoveTowards(modelTransform.position, destination, Time.deltaTime * displacementSpeed);

            yield return null;
        }

        modelTransform.position = initialPosition;

        if (OnDisplacementFinished != null)
        {
            OnDisplacementFinished();
        }
        StopCurrentDisplacement();
    }
}
