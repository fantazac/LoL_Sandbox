using System.Collections;
using UnityEngine;

public class ForcedActionManager : MonoBehaviour
{
    private Character unit;//TODO: Unit

    private int rotationSpeed;

    private IEnumerator currentForcedActionCoroutine;
    private AbilityBuff sourceAbilityBuffForForcedAction;
    private CrowdControlEffect currentCrowdControlEffect;
    private Unit caster;

    public bool IsBeingForced { get { return currentCrowdControlEffect != CrowdControlEffect.NONE; } }

    private void Start()
    {
        unit = GetComponent<Character>();
        if (unit.OrientationManager)//TODO: Unit
        {
            rotationSpeed = unit.OrientationManager.RotationSpeed;
        }
        else
        {
            rotationSpeed = 18;
        }
    }

    public void SetupForcedAction(CrowdControlEffect crowdControlEffect, AbilityBuff sourceAbilityBuff, Unit caster)
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

    private void StartForcedAction(CrowdControlEffect crowdControlEffect, AbilityBuff sourceAbilityBuff, Unit caster, IEnumerator coroutine)
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
            if (!unit.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                if (unit.MovementManager)
                {
                    unit.MovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }
    }

    private IEnumerator FinishCharm()
    {
        if (unit.MovementManager && !unit.MovementManager.IsMoving())
        {
            Transform casterTransform = caster.transform;
            Vector3 lastCasterPosition = casterTransform.position;
            while (transform.position != lastCasterPosition)
            {
                if (unit.MovementManager.IsMoving() || !unit.AbilityManager.CanUseMovement() || !unit.StatusManager.CanUseMovement() ||
                    Vector3.Distance(casterTransform.position, transform.position) <= unit.StatsManager.AttackRange.GetTotal())
                {
                    unit.MovementManager.SetMoveTowardsTarget(caster, unit.StatsManager.AttackRange.GetTotal(), true);
                    break;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastCasterPosition, Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                    unit.MovementManager.NotifyCharacterMoved();
                }

                yield return null;
            }

            if (!unit.MovementManager.IsMoving() && unit.AutoAttackManager)
            {
                unit.AutoAttackManager.EnableAutoAttack(true);
            }
        }
        else if (unit.AutoAttackManager)
        {
            unit.AutoAttackManager.EnableAutoAttack(true);
        }
    }

    private IEnumerator Fear()//TODO: Same as flee but every X seconds (like 0.5-1) you move towards RANDOM_POSITION
    {
        /*while (currentSourceAbilityBuffForForcedAction == sourceAbilityBuff)
        {
            if (!unit.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, RANDOM_POSITION, Time.deltaTime * unit.Stats.MovementSpeed.GetTotal());

                if (unit.CharacterMovementManager)
                {
                    unit.CharacterMovementManager.NotifyCharacterMoved();
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
            if (!unit.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, -Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                if (unit.MovementManager)
                {
                    unit.MovementManager.NotifyCharacterMoved();
                }

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -(casterTransform.position - transform.position), Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }
    }

    private void FinishFearAndFlee()
    {
        if (unit.MovementManager && !unit.MovementManager.IsMoving() && unit.AutoAttackManager)
        {
            unit.AutoAttackManager.EnableAutoAttack();
        }
    }

    private IEnumerator Taunt()
    {
        Transform casterTransform = caster.transform;
        while (true)
        {
            if (!unit.DisplacementManager.IsBeingDisplaced && unit.BasicAttack && !unit.BasicAttack.AttackIsInQueue)
            {
                if (Vector3.Distance(casterTransform.position, transform.position) > unit.StatsManager.AttackRange.GetTotal())
                {
                    transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                    if (unit.MovementManager)
                    {
                        unit.MovementManager.NotifyCharacterMoved();
                    }
                }
                else
                {
                    unit.BasicAttack.UseBasicAttackFromAutoAttackOrTaunt(caster);
                }

                if (unit.BasicAttack.BasicAttackCycle.AttackSpeedCycleIsReady)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
                }
            }

            yield return null;
        }
    }

    private void FinishTaunt()
    {
        if (unit.MovementManager && !unit.MovementManager.IsMoving())
        {
            unit.MovementManager.SetMoveTowardsTarget(caster, unit.StatsManager.AttackRange.GetTotal(), true);
        }
    }
}
