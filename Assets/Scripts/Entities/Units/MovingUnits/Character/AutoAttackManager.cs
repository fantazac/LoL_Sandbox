using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackManager : MonoBehaviour
{
    private Champion champion;
    private AttackRange attackRange;
    private readonly float biggerAttackRange;

    private IEnumerator currentAutoAttackCoroutine;

    private bool autoAttackEnabled;
    private List<Team> affectedTeams;
    private readonly List<Type> affectedUnitTypes;

    private AutoAttackManager()
    {
        autoAttackEnabled = true;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };

        biggerAttackRange = 750 * StaticObjects.MultiplyingFactor; //TODO: try to figure out actual value
    }

    public void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetEnemyTeams(allyTeam);
    }

    private void Start()
    {
        champion = GetComponent<Champion>();
        attackRange = champion.StatsManager.AttackRange;
    }

    public void EnableAutoAttack(bool forceEnable = false)
    {
        StopAutoAttack();

        if (!autoAttackEnabled && !forceEnable) return;

        currentAutoAttackCoroutine = AutoAttack();
        StartCoroutine(currentAutoAttackCoroutine);
    }

    public void EnableAutoAttackWithBiggerRange(bool forceEnable = false)
    {
        StopAutoAttack();

        if (!autoAttackEnabled && !forceEnable) return;

        currentAutoAttackCoroutine = AutoAttackWithBiggerRange();
        StartCoroutine(currentAutoAttackCoroutine);
    }

    public void StopAutoAttack()
    {
        if (currentAutoAttackCoroutine == null) return;

        StopCoroutine(currentAutoAttackCoroutine);
        currentAutoAttackCoroutine = null;
    }

    private IEnumerator AutoAttack()
    {
        if (!champion)
        {
            yield return null;
        }

        Unit autoAttackTarget = null;
        while (true)
        {
            if (CanUseAutoAttack())
            {
                if (!autoAttackTarget || Vector3.Distance(autoAttackTarget.transform.position, transform.position) > attackRange.GetTotal())
                {
                    autoAttackTarget = null;
                    float distance = float.MaxValue;
                    Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
                    foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, attackRange.GetTotal()))
                    {
                        Unit tempUnit = other.GetComponentInParent<Unit>();

                        if (!tempUnit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

                        float tempDistance = Vector3.Distance(tempUnit.transform.position, transform.position);

                        if (tempDistance >= distance) continue;

                        autoAttackTarget = tempUnit;
                        distance = tempDistance;
                    }
                }

                if (autoAttackTarget)
                {
                    champion.BasicAttack.UseBasicAttackFromAutoAttackOrTaunt(autoAttackTarget);
                }
            }

            yield return null;
        }
    }

    private IEnumerator AutoAttackWithBiggerRange()
    {
        if (!champion)
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
                foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, biggerAttackRange))
                {
                    Unit tempUnit = other.GetComponentInParent<Unit>();

                    if (!tempUnit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

                    float tempDistance = Vector3.Distance(tempUnit.transform.position, transform.position);

                    if (tempDistance >= distance) continue;

                    autoAttackTarget = tempUnit;
                    distance = tempDistance;
                }

                if (autoAttackTarget)
                {
                    champion.BasicAttack.SetupBasicAttack(autoAttackTarget, false);
                    break;
                }
            }

            yield return null;
        }

        currentAutoAttackCoroutine = null;
    }

    private bool CanUseAutoAttack()
    {
        return champion.StatusManager && champion.AbilityManager.CanUseBasicAttacks() && !champion.DisplacementManager.IsBeingDisplaced &&
               champion.StatusManager.CanUseBasicAttacks() && (!champion.ChampionMovementManager.IsMoving() || !champion.StatusManager.CanUseMovement()) &&
               !champion.BasicAttack.AttackIsInQueue && champion.BasicAttack.BasicAttackCycle.AttackSpeedCycleIsReady;
    }
}
