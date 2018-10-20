using System.Collections;
using UnityEngine;

public class EntityForcedActionManager : MonoBehaviour
{
    private Character entity;//TODO: Entity

    private int rotationSpeed;

    private AbilityBuff currentSourceAbilityBuffForForcedAction;

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

    public void SetupForcedAction(CrowdControlEffects crowdControlEffect, AbilityBuff sourceAbilityBuff, Entity caster, Vector3 destination, float speed)
    {
        switch (crowdControlEffect)
        {
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

    private void StartForcedAction(IEnumerator coroutine)//TODO: Displacments have to finish before taking effect
    {
        if (currentForcedAction != null)
        {
            StopCoroutine(currentForcedAction);
        }
        currentForcedAction = coroutine;
        StartCoroutine(currentForcedAction);
    }

    public void EndForcedAction(CrowdControlEffects crowdControlEffect, AbilityBuff sourceAbilityBuff)
    {
        if (crowdControlEffect == CrowdControlEffects.CHARM || crowdControlEffect == CrowdControlEffects.FEAR ||
            crowdControlEffect == CrowdControlEffects.FLEE || crowdControlEffect == CrowdControlEffects.TAUNT)
        {
            if (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
            {
                currentSourceAbilityBuffForForcedAction = null;
            }
        }
    }

    private IEnumerator Charm(AbilityBuff sourceAbilityBuff, Entity caster)
    {
        currentSourceAbilityBuffForForcedAction = sourceAbilityBuff;

        Transform casterTransform = caster.transform;
        while (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
        {
            if (!entity.EntityDisplacementManager.IsBeingDisplaced)
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
            if (!entity.EntityDisplacementManager.IsBeingDisplaced)
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
