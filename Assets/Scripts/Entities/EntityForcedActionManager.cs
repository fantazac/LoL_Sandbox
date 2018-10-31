﻿using System.Collections;
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
        if (entity.CharacterOrientationManager)//TODO: Entity
        {
            rotationSpeed = entity.CharacterOrientationManager.RotationSpeed;
        }
        else
        {
            rotationSpeed = 18;
        }
    }

    public void SetupForcedAction(CrowdControlEffects crowdControlEffect, AbilityBuff sourceAbilityBuff, Entity caster)
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

                if (entity.CharacterMovementManager)
                {
                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }

        if (entity.CharacterMovementManager && !entity.CharacterMovementManager.IsMoving())
        {
            Vector3 lastCasterPosition = caster.transform.position;
            while (transform.position != lastCasterPosition)
            {
                if (entity.CharacterMovementManager.IsMoving() || !entity.CharacterAbilityManager.CanUseMovement() || !entity.EntityStatusManager.CanUseMovement() ||
                    Vector3.Distance(casterTransform.position, transform.position) <= entity.EntityStats.AttackRange.GetTotal())
                {
                    entity.CharacterMovementManager.SetMoveTowardsTarget(caster, entity.EntityStats.AttackRange.GetTotal(), true);
                    break;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastCasterPosition, Time.deltaTime * entity.EntityStats.MovementSpeed.GetTotal());

                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                yield return null;
            }
        }

        if (entity.CharacterAutoAttackManager)
        {
            entity.CharacterAutoAttackManager.EnableAutoAttack(true);
        }

        currentForcedAction = null;
    }

    private IEnumerator Fear()//TODO: Same as fear but every X seconds (like 0.5-1) you move in a random direction
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

                if (entity.CharacterMovementManager)
                {
                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -(casterTransform.position - transform.position), Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }

        if (entity.CharacterAutoAttackManager)
        {
            entity.CharacterAutoAttackManager.EnableAutoAttack();
        }

        currentForcedAction = null;
    }

    private IEnumerator Taunt(Entity caster)//TODO: Basic attack caster and continue after if no commands were made
    {
        yield return null;
    }
}
