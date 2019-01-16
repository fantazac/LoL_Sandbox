using System.Collections;
using UnityEngine;

public class LucianBasicAttack : EmpoweredBasicAttack
{
    private float timeBeforePassiveShot;
    private WaitForSeconds delayPassiveShot;

    private bool isShootingPassiveShot;

    protected LucianBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 2500;

        timeBeforePassiveShot = 0.2f;
        delayPassiveShot = new WaitForSeconds(timeBeforePassiveShot);

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/Lucian/LucianBA";
        empoweredBasicAttackPrefabPath = "BasicAttacksPrefabs/Characters/Lucian/LucianBAPassive";
    }

    public override void StopBasicAttack(bool isCrowdControlled = false)
    {
        AttackIsInQueue = false;
        if (!isShootingPassiveShot && shootBasicAttackCoroutine != null)
        {
            StopCoroutine(shootBasicAttackCoroutine);
            shootBasicAttackCoroutine = null;
        }
        if (currentTarget != null)
        {
            if (isCrowdControlled)
            {
                StartBasicAttack();
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    protected override void UseBasicAttack(Unit target)
    {
        currentTarget = target;
        if (BasicAttackCycle.AttackSpeedCycleIsReady)
        {
            AttackIsInQueue = true;
            if (!isShootingPassiveShot && shootBasicAttackCoroutine != null)
            {
                StopCoroutine(shootBasicAttackCoroutine);
            }
            shootBasicAttackCoroutine = ShootBasicAttack(target);
            StartCoroutine(shootBasicAttackCoroutine);
        }
    }

    protected override IEnumerator ShootBasicAttack(Unit target)
    {
        SetupBeforeAttackDelay(target);

        yield return delayAttack;

        isShootingPassiveShot = true;

        if (!empoweringAbilityWasActiveOnBasicAttackCast)
        {
            empoweringAbilityWasActiveOnBasicAttackCast = unit.BuffManager.IsAffectedByBuff(basicAttackEmpoweringAbility.AbilityBuffs[0]);
        }

        SetupAfterAttackDelay(target, basicAttackPrefab);

        if (empoweringAbilityWasActiveOnBasicAttackCast)
        {
            empoweringAbilityWasActiveOnBasicAttackCast = false;
            basicAttackEmpoweringAbility.AbilityBuffs[0].ConsumeBuff(unit);

            yield return delayPassiveShot;

            ProjectileUnitTargeted projectile2 = (Instantiate(empoweredBasicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
            projectile2.transform.LookAt(target.transform);
            projectile2.ShootProjectile(affectedTeams, target, speed, AttackIsCritical.CheckIfAttackIsCritical(unit.StatsManager.CriticalStrikeChance.GetTotal()));
            projectile2.OnAbilityEffectHit += PassiveBasicAttackHit;
        }

        isShootingPassiveShot = false;
        shootBasicAttackCoroutine = null;
    }

    protected override void EmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike)
    {
        basicAttackEmpoweringAbility.UseAbility(unitHit);
    }

    private void PassiveBasicAttackHit(AbilityEffect basicAttackProjectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(unit.StatusManager.IsBlinded() || willMiss))
        {
            basicAttackEmpoweringAbility.OnEmpoweredBasicAttackHit(unitHit, isACriticalStrike);
        }
        Destroy(basicAttackProjectile.gameObject);
    }
}
