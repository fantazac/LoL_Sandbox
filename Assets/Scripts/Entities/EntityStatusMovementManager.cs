using System.Collections;
using UnityEngine;

public class EntityStatusMovementManager : MonoBehaviour
{
    private Character entity;//TODO: Entity

    private bool isBeingDisplaced;
    private int rotationSpeed;

    private AbilityBuff currentSourceAbilityBuffForDisplacement;
    private AbilityBuff currentSourceAbilityBuffForForcedAction;

    private IEnumerator currentDisplacement;
    private IEnumerator currentForcedAction;

    private void Start()
    {
        entity = GetComponent<Character>();
        if (entity.CharacterOrientation)//TODO: Entity
        {
            rotationSpeed = entity.CharacterOrientation.RotationSpeed;
        }
        else
        {
            rotationSpeed = 18;
        }
    }

    public void SetupMovementBlock(CrowdControlEffects crowdControlEffect, AbilityBuff sourceAbilityBuff, Entity caster, Vector3 destination, float durationOrDistance, float speed)
    {
        switch (crowdControlEffect)
        {
            //Displacements
            case CrowdControlEffects.KNOCKASIDE:
                StartDisplacement(Knockaside(sourceAbilityBuff));
                break;
            case CrowdControlEffects.KNOCKBACK:
                StartDisplacement(Knockback(sourceAbilityBuff, destination, durationOrDistance, speed));
                break;
            case CrowdControlEffects.KNOCKUP:
                StartDisplacement(Knockup(sourceAbilityBuff, durationOrDistance, speed));
                break;
            case CrowdControlEffects.PULL:
                StartDisplacement(Pull(sourceAbilityBuff));
                break;
            //Forced actions
            case CrowdControlEffects.CHARM:
                StartForcedAction(Charm(sourceAbilityBuff, caster));
                break;
            case CrowdControlEffects.FEAR:
                StartForcedAction(Fear());
                break;
            case CrowdControlEffects.FLEE:
                StartForcedAction(Flee(sourceAbilityBuff, caster));
                break;
            case CrowdControlEffects.TAUNT:
                StartForcedAction(Taunt(caster));
                break;
        }
    }

    public void EndMovementBlock(CrowdControlEffects crowdControlEffect, AbilityBuff sourceAbilityBuff)
    {
        if (crowdControlEffect == CrowdControlEffects.KNOCKASIDE || crowdControlEffect == CrowdControlEffects.KNOCKBACK ||
            crowdControlEffect == CrowdControlEffects.KNOCKUP || crowdControlEffect == CrowdControlEffects.PULL)
        {
            EndDisplacement(sourceAbilityBuff);
        }
        else if (crowdControlEffect == CrowdControlEffects.CHARM || crowdControlEffect == CrowdControlEffects.FEAR ||
            crowdControlEffect == CrowdControlEffects.FLEE || crowdControlEffect == CrowdControlEffects.TAUNT)
        {
            EndForcedAction(sourceAbilityBuff);
        }
    }

    private void EndDisplacement(AbilityBuff sourceAbilityBuff)
    {
        if (currentSourceAbilityBuffForDisplacement == sourceAbilityBuff)
        {
            currentSourceAbilityBuffForDisplacement = null;
        }
    }

    private void EndForcedAction(AbilityBuff sourceAbilityBuff)
    {
        if (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
        {
            currentSourceAbilityBuffForForcedAction = null;
        }
    }

    private void StartDisplacement(IEnumerator coroutine)
    {
        if (currentDisplacement != null)
        {
            StopCoroutine(currentDisplacement);
        }
        currentDisplacement = coroutine;
        isBeingDisplaced = true;
        StartCoroutine(currentDisplacement);
    }

    private void FinishDisplacement(AbilityBuff sourceAbilityBuff)
    {
        sourceAbilityBuff.ConsumeBuff(entity);
        isBeingDisplaced = false;
        currentDisplacement = null;
    }

    private IEnumerator Knockaside(AbilityBuff sourceAbilityBuff)
    {
        yield return null;

        FinishDisplacement(sourceAbilityBuff);
    }

    private IEnumerator Knockback(AbilityBuff sourceAbilityBuff, Vector3 destination, float distance, float knockbackSpeed)
    {
        currentSourceAbilityBuffForDisplacement = sourceAbilityBuff;

        destination += transform.position;

        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * knockbackSpeed);

            if (entity is Character && !(entity is Dummy))
            {
                ((Character)entity).CharacterMovement.NotifyCharacterMoved();
            }

            yield return null;
        }

        FinishDisplacement(sourceAbilityBuff);
    }

    private IEnumerator Knockup(AbilityBuff sourceAbilityBuff, float duration, float knockupSpeed)
    {
        currentSourceAbilityBuffForDisplacement = sourceAbilityBuff;

        Vector3 initialPosition = transform.position;
        Transform modelTransform = entity.EntityModelObject.transform;
        Vector3 up = Vector3.up * 10;

        while (currentSourceAbilityBuffForDisplacement == sourceAbilityBuff)
        {
            modelTransform.position = Vector3.MoveTowards(modelTransform.position, modelTransform.position + up, Time.deltaTime * knockupSpeed);

            yield return null;
        }

        entity.EntityModelObject.transform.position = initialPosition;

        FinishDisplacement(sourceAbilityBuff);
    }

    private IEnumerator Pull(AbilityBuff sourceAbilityBuff)
    {
        yield return null;

        FinishDisplacement(sourceAbilityBuff);
    }

    private void StartForcedAction(IEnumerator coroutine)//TODO: Dashes have to finish before taking effect
    {
        if (currentForcedAction != null)
        {
            StopCoroutine(currentForcedAction);
        }
        currentForcedAction = coroutine;
        StartCoroutine(currentForcedAction);
    }

    private IEnumerator Charm(AbilityBuff sourceAbilityBuff, Entity caster)
    {
        currentSourceAbilityBuffForForcedAction = sourceAbilityBuff;

        Transform casterTransform = caster.transform;
        while (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
        {
            if (!isBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, Time.deltaTime * entity.EntityStats.MovementSpeed.GetTotal());

                if (entity.CharacterMovement)
                {
                    entity.CharacterMovement.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }

        if (entity.CharacterMovement && !entity.CharacterMovement.IsMoving())
        {
            Vector3 lastCasterPosition = caster.transform.position;
            while (transform.position != lastCasterPosition)
            {
                if (entity.CharacterMovement.IsMoving() || entity.CharacterAbilityManager.IsUsingAbilityPreventingMovement() || !entity.EntityStatusManager.CanUseMovement() ||
                    Vector3.Distance(casterTransform.position, transform.position) <= entity.EntityStats.AttackRange.GetTotal())
                {
                    entity.CharacterMovement.SetMoveTowardsTarget(caster, entity.EntityStats.AttackRange.GetTotal(), true);
                    break;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastCasterPosition, Time.deltaTime * entity.EntityStats.MovementSpeed.GetTotal());

                    entity.CharacterMovement.NotifyCharacterMoved();
                }

                yield return null;
            }
        }

        if (entity.CharacterAutoAttack)
        {
            entity.CharacterAutoAttack.EnableAutoAttack(true);
        }

        currentForcedAction = null;
    }

    private IEnumerator Fear()//TODO
    {
        yield return null;
    }

    private IEnumerator Flee(AbilityBuff sourceAbilityBuff, Entity caster)
    {
        currentSourceAbilityBuffForForcedAction = sourceAbilityBuff;

        Transform casterTransform = caster.transform;
        while (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
        {
            if (!isBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, -Time.deltaTime * entity.EntityStats.MovementSpeed.GetTotal());

                if (entity.CharacterMovement)
                {
                    entity.CharacterMovement.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -(casterTransform.position - transform.position), Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }

        if (entity.CharacterAutoAttack)
        {
            entity.CharacterAutoAttack.EnableAutoAttack();//TODO: Forced?
        }

        currentForcedAction = null;
    }

    private IEnumerator Taunt(Entity caster)
    {
        yield return null;
    }
}
