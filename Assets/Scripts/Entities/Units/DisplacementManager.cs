using System.Collections;
using UnityEngine;

public class DisplacementManager : MonoBehaviour
{
    private Unit unit;

    private IEnumerator currentDisplacementCoroutine;
    private AbilityBuff sourceAbilityBuffForCurrentDisplacement;

    public bool IsBeingDisplaced => currentDisplacementCoroutine != null;

    public delegate void OnDisplacementFinishedHandler();
    public event OnDisplacementFinishedHandler OnDisplacementFinished;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void SetupDisplacement(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff = null, bool isAKnockUp = false)
    {
        StopCurrentDisplacement();

        destination += transform.position;
        currentDisplacementCoroutine =
            isAKnockUp ? KnockUp(destination, displacementSpeed, sourceAbilityBuff) : Displacement(destination, displacementSpeed, sourceAbilityBuff);
        sourceAbilityBuffForCurrentDisplacement = sourceAbilityBuff;
        StartCoroutine(currentDisplacementCoroutine);
    }

    public void StopCurrentDisplacement()
    {
        OnDisplacementFinished = null;

        if (currentDisplacementCoroutine == null) return;

        StopCoroutine(currentDisplacementCoroutine);
        currentDisplacementCoroutine = null;
        if (sourceAbilityBuffForCurrentDisplacement)
        {
            sourceAbilityBuffForCurrentDisplacement.ConsumeBuff(unit);
            sourceAbilityBuffForCurrentDisplacement = null;
        }

        unit.ModelObject.transform.position = transform.position;
    }

    private IEnumerator Displacement(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * displacementSpeed);

            if (unit is Champion champion) //TODO
            {
                champion.ChampionMovementManager.NotifyChampionMoved();
            }

            yield return null;
        }

        OnDisplacementFinished?.Invoke();
        StopCurrentDisplacement();
    }

    private IEnumerator KnockUp(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff)
    {
        Vector3 initialPosition = transform.position;
        Transform modelTransform = unit.ModelObject.transform;

        while (modelTransform.position != destination) //up
        {
            modelTransform.position = Vector3.MoveTowards(modelTransform.position, destination, Time.deltaTime * displacementSpeed);

            yield return null;
        }

        destination = initialPosition;

        while (modelTransform.position != destination) //down
        {
            modelTransform.position = Vector3.MoveTowards(modelTransform.position, destination, Time.deltaTime * displacementSpeed);

            yield return null;
        }

        modelTransform.position = initialPosition;

        OnDisplacementFinished?.Invoke();
        StopCurrentDisplacement();
    }
}
