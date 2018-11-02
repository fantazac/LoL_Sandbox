using System.Collections;
using UnityEngine;

public class EntityForcedActionManager : MonoBehaviour
{
    private Character entity;//TODO: Entity

    private int rotationSpeed;

    private IEnumerator currentForcedActionCoroutine;
    private AbilityBuff sourceAbilityBuffForForcedAction;
    private CrowdControlEffect currentCrowdControlEffect;
    private Entity caster;

    public bool IsBeingForced { get { return currentCrowdControlEffect != CrowdControlEffect.NONE; } }

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

    public void SetupForcedAction(CrowdControlEffect crowdControlEffect, AbilityBuff sourceAbilityBuff, Entity caster)
    {
        switch (crowdControlEffect)
        {
            case CrowdControlEffect.CHARM:
                StartForcedAction(crowdControlEffect, sourceAbilityBuff, caster, Charm());
                break;
            case CrowdControlEffect.FEAR:
                StartForcedAction(crowdControlEffect, sourceAbilityBuff, caster, Fear());
                break;
            case CrowdControlEffect.FLEE:
                StartForcedAction(crowdControlEffect, sourceAbilityBuff, caster, Flee());
                break;
            case CrowdControlEffect.TAUNT:
                StartForcedAction(crowdControlEffect, sourceAbilityBuff, caster, Taunt());
                break;
        }
    }

    private void StartForcedAction(CrowdControlEffect crowdControlEffect, AbilityBuff sourceAbilityBuff, Entity caster, IEnumerator coroutine)
    {
        StopCurrentForcedAction(sourceAbilityBuffForForcedAction);

        this.caster = caster;
        sourceAbilityBuffForForcedAction = sourceAbilityBuff;
        currentCrowdControlEffect = crowdControlEffect;
        currentForcedActionCoroutine = coroutine;
        StartCoroutine(currentForcedActionCoroutine);
    }

    public void StopCurrentForcedAction(AbilityBuff sourceAbilityBuff)
    {
        if (sourceAbilityBuffForForcedAction == sourceAbilityBuff)//This is only useful when an AbilityBuff is trying to consume itself, as calls from this class will always be true
        {
            if (currentForcedActionCoroutine != null)
            {
                StopCoroutine(currentForcedActionCoroutine);
                currentForcedActionCoroutine = null;
            }
            CrowdControlEffect tempCrowdControlEffect = currentCrowdControlEffect;
            currentCrowdControlEffect = CrowdControlEffect.NONE;
            if (tempCrowdControlEffect == CrowdControlEffect.CHARM)
            {
                currentForcedActionCoroutine = FinishCharm();
                StartCoroutine(currentForcedActionCoroutine);
            }
            else if (tempCrowdControlEffect == CrowdControlEffect.FLEE || tempCrowdControlEffect == CrowdControlEffect.FEAR)
            {
                FinishFearAndFlee();
            }
            else if (tempCrowdControlEffect == CrowdControlEffect.TAUNT)
            {
                FinishTaunt();
            }
            sourceAbilityBuffForForcedAction = null;
        }
    }

    private IEnumerator Charm()
    {
        Transform casterTransform = caster.transform;
        while (true)
        {
            if (!entity.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, Time.deltaTime * entity.EntityStatsManager.MovementSpeed.GetTotal());

                if (entity.CharacterMovementManager)
                {
                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }
    }

    private IEnumerator FinishCharm()
    {
        if (entity.CharacterMovementManager && !entity.CharacterMovementManager.IsMoving())
        {
            Transform casterTransform = caster.transform;
            Vector3 lastCasterPosition = casterTransform.position;
            while (transform.position != lastCasterPosition)
            {
                if (entity.CharacterMovementManager.IsMoving() || !entity.CharacterAbilityManager.CanUseMovement() || !entity.EntityStatusManager.CanUseMovement() ||
                    Vector3.Distance(casterTransform.position, transform.position) <= entity.EntityStatsManager.AttackRange.GetTotal())
                {
                    entity.CharacterMovementManager.SetMoveTowardsTarget(caster, entity.EntityStatsManager.AttackRange.GetTotal(), true);
                    break;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastCasterPosition, Time.deltaTime * entity.EntityStatsManager.MovementSpeed.GetTotal());

                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                yield return null;
            }

            if (!entity.CharacterMovementManager.IsMoving() && entity.CharacterAutoAttackManager)
            {
                entity.CharacterAutoAttackManager.EnableAutoAttack(true);
            }
        }
        else if (entity.CharacterAutoAttackManager)
        {
            entity.CharacterAutoAttackManager.EnableAutoAttack(true);
        }
    }

    private IEnumerator Fear()//TODO: Same as flee but every X seconds (like 0.5-1) you move towards RANDOM_POSITION
    {
        /*while (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
        {
            if (!entity.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, RANDOM_POSITION, Time.deltaTime * entity.EntityStats.MovementSpeed.GetTotal());

                if (entity.CharacterMovementManager)
                {
                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, RANDOM_POSITION - transform.position, Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }*/

        yield return null;//To remove
    }

    private IEnumerator Flee()
    {
        Transform casterTransform = caster.transform;
        while (true)
        {
            if (!entity.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, -Time.deltaTime * entity.EntityStatsManager.MovementSpeed.GetTotal());

                if (entity.CharacterMovementManager)
                {
                    entity.CharacterMovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -(casterTransform.position - transform.position), Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }
    }

    private void FinishFearAndFlee()
    {
        if (entity.CharacterMovementManager && !entity.CharacterMovementManager.IsMoving() && entity.CharacterAutoAttackManager)
        {
            entity.CharacterAutoAttackManager.EnableAutoAttack();
        }
    }

    private IEnumerator Taunt()
    {
        Transform casterTransform = caster.transform;
        while (true)
        {
            if (!entity.EntityDisplacementManager.IsBeingDisplaced && entity.EntityBasicAttack && !entity.EntityBasicAttack.AttackIsInQueue)
            {
                if (Vector3.Distance(casterTransform.position, transform.position) > entity.EntityStatsManager.AttackRange.GetTotal())
                {
                    transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, Time.deltaTime * entity.EntityStatsManager.MovementSpeed.GetTotal());

                    if (entity.CharacterMovementManager)
                    {
                        entity.CharacterMovementManager.NotifyCharacterMoved();
                    }
                }
                else
                {
                    entity.EntityBasicAttack.UseBasicAttackFromAutoAttackOrTaunt(caster);
                }

                if (entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
                }
            }

            yield return null;
        }
    }

    private void FinishTaunt()
    {
        if (entity.CharacterMovementManager && !entity.CharacterMovementManager.IsMoving())
        {
            entity.CharacterMovementManager.SetMoveTowardsTarget(caster, entity.EntityStatsManager.AttackRange.GetTotal(), true);
        }
    }
}
