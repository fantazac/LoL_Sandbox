using System.Collections;
using UnityEngine;

public class ForcedActionManager : MonoBehaviour
{
    private Unit unit;
    private Champion champion;

    private int rotationSpeed;

    private IEnumerator currentForcedActionCoroutine;
    private AbilityBuff sourceAbilityBuffForForcedAction;
    private StatusEffect currentStatusEffect;
    private Unit caster;

    public bool IsBeingForced => currentStatusEffect != StatusEffect.NONE;

    private void Start()
    {
        unit = GetComponent<Unit>();
        if (unit is Champion c) //TODO: all units will have an orientation manager
        {
            champion = c;
            rotationSpeed = c.OrientationManager.RotationSpeed;
        }
        else
        {
            rotationSpeed = 18;
        }
    }

    public void SetupForcedAction(StatusEffect statusEffect, AbilityBuff sourceAbilityBuff, Unit caster)
    {
        switch (statusEffect)
        {
            case StatusEffect.CHARM:
                StartForcedAction(statusEffect, sourceAbilityBuff, caster, Charm());
                break;
            case StatusEffect.FEAR:
                StartForcedAction(statusEffect, sourceAbilityBuff, caster, Fear());
                break;
            case StatusEffect.FLEE:
                StartForcedAction(statusEffect, sourceAbilityBuff, caster, Flee());
                break;
            case StatusEffect.TAUNT:
                StartForcedAction(statusEffect, sourceAbilityBuff, caster, Taunt());
                break;
        }
    }

    private void StartForcedAction(StatusEffect statusEffect, AbilityBuff sourceAbilityBuff, Unit caster, IEnumerator coroutine)
    {
        StopCurrentForcedAction(sourceAbilityBuffForForcedAction);

        this.caster = caster;
        sourceAbilityBuffForForcedAction = sourceAbilityBuff;
        currentStatusEffect = statusEffect;
        currentForcedActionCoroutine = coroutine;
        StartCoroutine(currentForcedActionCoroutine);
    }

    public void StopCurrentForcedAction(AbilityBuff sourceAbilityBuff)
    {
        if (sourceAbilityBuffForForcedAction != sourceAbilityBuff) return;

        if (currentForcedActionCoroutine != null)
        {
            StopCoroutine(currentForcedActionCoroutine);
            currentForcedActionCoroutine = null;
        }

        StatusEffect tempStatusEffect = currentStatusEffect;
        currentStatusEffect = StatusEffect.NONE;
        switch (tempStatusEffect)
        {
            case StatusEffect.CHARM:
                currentForcedActionCoroutine = FinishCharm();
                StartCoroutine(currentForcedActionCoroutine);
                break;
            case StatusEffect.FLEE:
            case StatusEffect.FEAR:
                FinishFearAndFlee();
                break;
            case StatusEffect.TAUNT:
                FinishTaunt();
                break;
        }

        sourceAbilityBuffForForcedAction = null;
    }

    private IEnumerator Charm()
    {
        Transform casterTransform = caster.transform;
        while (true)
        {
            if (!unit.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                if (unit is Champion)
                {
                    champion.ChampionMovementManager.NotifyChampionMoved();
                }

                transform.rotation =
                    Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }
    }

    private IEnumerator FinishCharm()
    {
        if (champion.MovementManager && !champion.ChampionMovementManager.IsMoving())
        {
            Transform casterTransform = caster.transform;
            Vector3 lastCasterPosition = casterTransform.position;
            while (transform.position != lastCasterPosition)
            {
                if (champion.ChampionMovementManager.IsMoving() || !champion.AbilityManager.CanUseMovement() || !unit.StatusManager.CanUseMovement() ||
                    Vector3.Distance(casterTransform.position, transform.position) <= unit.StatsManager.AttackRange.GetTotal())
                {
                    champion.ChampionMovementManager.SetMoveTowardsTarget(caster, unit.StatsManager.AttackRange.GetTotal(), true);
                    break;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastCasterPosition, Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                    champion.ChampionMovementManager.NotifyChampionMoved();
                }

                yield return null;
            }

            if (!champion.ChampionMovementManager.IsMoving() && champion.AutoAttackManager)
            {
                champion.AutoAttackManager.EnableAutoAttack(true);
            }
        }
        else if (champion.AutoAttackManager)
        {
            champion.AutoAttackManager.EnableAutoAttack(true);
        }
    }

    private IEnumerator Fear() //TODO: Same as flee but every X seconds (like 0.5-1) you move towards RANDOM_POSITION
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

        yield return null; //To remove
    }

    private IEnumerator Flee()
    {
        Transform casterTransform = caster.transform;
        while (true)
        {
            if (!unit.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, casterTransform.position, -Time.deltaTime * unit.StatsManager.MovementSpeed.GetTotal());

                if (champion.MovementManager)
                {
                    champion.ChampionMovementManager.NotifyChampionMoved();
                }

                transform.rotation =
                    Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -(casterTransform.position - transform.position), Time.deltaTime * rotationSpeed, 0));
            }

            yield return null;
        }
    }

    private void FinishFearAndFlee()
    {
        if (champion.MovementManager && !champion.ChampionMovementManager.IsMoving() && champion.AutoAttackManager)
        {
            champion.AutoAttackManager.EnableAutoAttack();
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

                    if (champion.MovementManager)
                    {
                        champion.ChampionMovementManager.NotifyChampionMoved();
                    }
                }
                else
                {
                    unit.BasicAttack.UseBasicAttackFromAutoAttackOrTaunt(caster);
                }

                if (unit.BasicAttack.BasicAttackCycle.AttackSpeedCycleIsReady)
                {
                    transform.rotation =
                        Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, casterTransform.position - transform.position, Time.deltaTime * rotationSpeed, 0));
                }
            }

            yield return null;
        }
    }

    private void FinishTaunt()
    {
        if (champion && champion.MovementManager && !champion.ChampionMovementManager.IsMoving())
        {
            champion.ChampionMovementManager.SetMoveTowardsTarget(caster, unit.StatsManager.AttackRange.GetTotal(), true);
        }
    }
}
