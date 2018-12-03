using System.Collections;
using UnityEngine;

public class AutoAttackManager : MonoBehaviour
{
    private Character character;
    private AttackRange attackRange;
    private float biggerAttackRange;

    private IEnumerator currentAutoAttackCoroutine;

    private bool autoAttackEnabled;
    private AbilityAffectedUnitType affectedUnitType;

    private AutoAttackManager()
    {
        autoAttackEnabled = true;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        biggerAttackRange = 750 * StaticObjects.MultiplyingFactor;//TODO: try to figure out actual value
    }

    private void Start()
    {
        character = GetComponent<Character>();
        attackRange = character.StatsManager.AttackRange;
        EnableAutoAttack();
    }

    public void EnableAutoAttack(bool forceEnable = false)
    {
        StopAutoAttack();
        if (autoAttackEnabled || forceEnable)
        {
            currentAutoAttackCoroutine = AutoAttack();
            StartCoroutine(currentAutoAttackCoroutine);
        }
    }

    public void EnableAutoAttackWithBiggerRange(bool forceEnable = false)
    {
        StopAutoAttack();
        if (autoAttackEnabled || forceEnable)
        {
            currentAutoAttackCoroutine = AutoAttackWithBiggerRange();
            StartCoroutine(currentAutoAttackCoroutine);
        }
    }

    public void StopAutoAttack()
    {
        if (currentAutoAttackCoroutine != null)
        {
            StopCoroutine(currentAutoAttackCoroutine);
            currentAutoAttackCoroutine = null;
        }
    }

    private IEnumerator AutoAttack()
    {
        if (!character)
        {
            yield return null;
        }

        Vector3 groundPosition;

        Unit autoAttackTarget = null;
        while (true)
        {
            if (CanUseAutoAttack())
            {
                if (autoAttackTarget == null || Vector3.Distance(autoAttackTarget.transform.position, transform.position) > attackRange.GetTotal())
                {
                    autoAttackTarget = null;
                    float distance = float.MaxValue;
                    groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
                    foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, attackRange.GetTotal()))
                    {
                        Unit tempUnit = collider.GetComponentInParent<Unit>();
                        if (tempUnit != null && TargetIsValid.CheckIfTargetIsValid(tempUnit, affectedUnitType, character.Team))
                        {
                            float tempDistance = Vector3.Distance(tempUnit.transform.position, transform.position);
                            if (tempDistance < distance)
                            {
                                autoAttackTarget = tempUnit;
                                distance = tempDistance;
                            }
                        }
                    }
                }

                if (autoAttackTarget != null)
                {
                    character.BasicAttack.UseBasicAttackFromAutoAttackOrTaunt(autoAttackTarget);
                }
            }

            yield return null;
        }
    }

    private IEnumerator AutoAttackWithBiggerRange()
    {
        if (!character)
        {
            yield return null;
        }

        Unit autoAttackTarget = null;
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        while (true)
        {
            if (CanUseAutoAttack())
            {
                float distance = float.MaxValue;
                foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, biggerAttackRange))
                {
                    Unit tempUnit = collider.GetComponentInParent<Unit>();
                    if (tempUnit != null && TargetIsValid.CheckIfTargetIsValid(tempUnit, affectedUnitType, character.Team))
                    {
                        float tempDistance = Vector3.Distance(tempUnit.transform.position, transform.position);
                        if (tempDistance < distance)
                        {
                            autoAttackTarget = tempUnit;
                            distance = tempDistance;
                        }
                    }
                }

                if (autoAttackTarget != null)
                {
                    character.BasicAttack.SetupBasicAttack(autoAttackTarget, false);
                    break;
                }
            }

            yield return null;
        }

        currentAutoAttackCoroutine = null;
    }

    protected bool CanUseAutoAttack()
    {
        return character.StatusManager && character.AbilityManager.CanUseBasicAttacks() && !character.DisplacementManager.IsBeingDisplaced &&
                character.StatusManager.CanUseBasicAttacks() && (!character.MovementManager.IsMoving() || !character.StatusManager.CanUseMovement()) &&
                !character.BasicAttack.AttackIsInQueue && character.BasicAttack.BasicAttackCycle.AttackSpeedCycleIsReady;
    }
}
