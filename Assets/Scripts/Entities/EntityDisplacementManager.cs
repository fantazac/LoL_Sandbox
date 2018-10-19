using System.Collections;
using UnityEngine;

public class EntityDisplacementManager : MonoBehaviour
{
    private Character entity;//TODO: Entity

    private IEnumerator currentDisplacementCoroutine;
    private AbilityBuff sourceAbilityBuffForCurrentDisplacement;

    public bool isBeingDisplaced { get { return currentDisplacementCoroutine != null; } }

    private void Start()
    {
        entity = GetComponent<Character>();
    }

    public void SetupDisplacement(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff = null, bool isAKnockup = false)
    {
        StopCurrentDisplacement();

        destination += transform.position;
        currentDisplacementCoroutine = isAKnockup ? Knockup(destination, displacementSpeed, sourceAbilityBuff) : Displacement(destination, displacementSpeed, sourceAbilityBuff);
        sourceAbilityBuffForCurrentDisplacement = sourceAbilityBuff;
        StartCoroutine(currentDisplacementCoroutine);
    }

    private void StopCurrentDisplacement()
    {
        if (currentDisplacementCoroutine != null)
        {
            StopCoroutine(currentDisplacementCoroutine);
            if (sourceAbilityBuffForCurrentDisplacement)
            {
                sourceAbilityBuffForCurrentDisplacement.ConsumeBuff(entity);
            }
        }
    }

    private IEnumerator Displacement(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * displacementSpeed);

            if (entity.CharacterMovement)
            {
                entity.CharacterMovement.NotifyCharacterMoved();
            }

            yield return null;
        }

        if (sourceAbilityBuff)
        {
            sourceAbilityBuff.ConsumeBuff(entity);
        }

        currentDisplacementCoroutine = null;
    }

    private IEnumerator Knockup(Vector3 destination, float displacementSpeed, AbilityBuff sourceAbilityBuff)
    {
        Vector3 initialPosition = transform.position;
        Transform modelTransform = entity.EntityModelObject.transform;

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

        if (sourceAbilityBuff)
        {
            sourceAbilityBuff.ConsumeBuff(entity);
        }

        currentDisplacementCoroutine = null;
    }
}
