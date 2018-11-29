using System.Collections;
using UnityEngine;

public class LucianBasicAttack : EmpoweredCharacterBasicAttack
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

    protected override void UseBasicAttack(Entity target)
    {
        currentTarget = target;
        if (EntityBasicAttackCycle.AttackSpeedCycleIsReady)
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

    protected override IEnumerator ShootBasicAttack(Entity target)
    {
        SetupBeforeAttackDelay(target);

        yield return delayAttack;

        isShootingPassiveShot = true;

        if (!empoweringAbilityWasActiveOnBasicAttackCast)
        {
            empoweringAbilityWasActiveOnBasicAttackCast = entity.EntityBuffManager.IsAffectedByBuff(basicAttackEmpoweringAbility.AbilityBuffs[0]);
        }

        SetupAfterAttackDelay(target, basicAttackPrefab);

        if (empoweringAbilityWasActiveOnBasicAttackCast)
        {
            empoweringAbilityWasActiveOnBasicAttackCast = false;
            basicAttackEmpoweringAbility.AbilityBuffs[0].ConsumeBuff(entity);

            yield return delayPassiveShot;

            ProjectileUnitTargeted projectile2 = (Instantiate(empoweredBasicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
            projectile2.transform.LookAt(target.transform);
            projectile2.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStatsManager.CriticalStrikeChance.GetTotal()));
            projectile2.OnAbilityEffectHit += PassiveBasicAttackHit;
        }

        isShootingPassiveShot = false;
        shootBasicAttackCoroutine = null;
    }

    protected override void OnEmpoweredBasicAttackHit(Entity entityHit, bool isACriticalStrike)
    {
        basicAttackEmpoweringAbility.UseAbility(entityHit);
    }

    private void PassiveBasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            basicAttackEmpoweringAbility.OnEmpoweredBasicAttackHit(entityHit, isACriticalStrike);
        }
        Destroy(basicAttackProjectile.gameObject);
    }
}
