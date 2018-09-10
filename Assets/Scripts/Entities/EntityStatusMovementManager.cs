using System.Collections;
using System.Collections.Generic;
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

    public void SetupMovementBlock(CrowdControlEffects crowdControlEffect, AbilityBuff sourceAbilityBuff, Entity caster, Vector3 position, float duration)
    {
        switch (crowdControlEffect)
        {
            //Displacements
            case CrowdControlEffects.KNOCKASIDE:
                StartDisplacement(Knockaside());
                break;
            case CrowdControlEffects.KNOCKBACK:
                StartDisplacement(Knockback());
                break;
            case CrowdControlEffects.KNOCKUP:
                StartDisplacement(Knockup(sourceAbilityBuff, duration));
                break;
            case CrowdControlEffects.PULL:
                StartDisplacement(Pull());
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

    private IEnumerator Knockaside()
    {
        yield return null;

        currentDisplacement = null;
    }

    private IEnumerator Knockback()
    {
        yield return null;

        currentDisplacement = null;
    }

    private IEnumerator Knockup(AbilityBuff sourceAbilityBuff, float duration)
    {
        currentSourceAbilityBuffForDisplacement = sourceAbilityBuff;

        Vector3 initialPosition = transform.position;
        Vector3 up = Vector3.up * 10;
        float upSpeed = 2;

        while (currentSourceAbilityBuffForDisplacement == sourceAbilityBuff)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + up, Time.deltaTime * upSpeed);

            yield return null;
        }

        transform.position = initialPosition;

        currentDisplacement = null;
    }

    private IEnumerator Pull()
    {
        yield return null;

        currentDisplacement = null;
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
