using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucianBasicAttack : CharacterBasicAttack
{
    private string passiveBasicAttackPrefabPath;
    private GameObject passiveBasicAttackPrefab;

    private float timeBeforePassiveShot;
    private WaitForSeconds delayPassiveShot;

    private Lucian_P passive;

    private bool isShootingPassiveShot;
    private bool passiveWasActiveOnBasicAttackCast;

    protected LucianBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 2500;

        timeBeforePassiveShot = 0.2f;
        delayPassiveShot = new WaitForSeconds(timeBeforePassiveShot);

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/Lucian/LucianBA";
        passiveBasicAttackPrefabPath = "BasicAttacksPrefabs/Characters/Lucian/LucianBAPassive";
    }

    public void SetPassive(Lucian_P lucianPassive)
    {
        passive = lucianPassive;
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        passiveBasicAttackPrefab = Resources.Load<GameObject>(passiveBasicAttackPrefabPath);
    }

    public override void StopBasicAttack(bool isCrowdControlled = false)
    {
        attackIsInQueue = false;
        if (currentTarget != null)
        {
            if (!isShootingPassiveShot)
            {
                StopAllCoroutines();
            }
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
        if (entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
        {
            attackIsInQueue = true;
            if (!isShootingPassiveShot)
            {
                StopAllCoroutines();
            }
            StartCoroutine(ShootBasicAttack(target));
        }
    }

    protected override IEnumerator ShootBasicAttack(Entity target)
    {
        passiveWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(passive.AbilityBuffs[0]) != null;

        yield return delayAttack;

        isShootingPassiveShot = true;

        if (!passiveWasActiveOnBasicAttackCast)
        {
            passiveWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(passive.AbilityBuffs[0]) != null;
        }

        entity.EntityBasicAttackCycle.LockBasicAttack();
        attackIsInQueue = false;

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()));
        if (passiveWasActiveOnBasicAttackCast)
        {
            projectile.OnProjectileUnitTargetedHit += BasicAttackHit;
        }
        else
        {
            projectile.OnProjectileUnitTargetedHit += base.BasicAttackHit;
        }

        if (passiveWasActiveOnBasicAttackCast)
        {
            passiveWasActiveOnBasicAttackCast = false;
            passive.AbilityBuffs[0].ConsumeBuff(entity);

            yield return delayPassiveShot;

            ProjectileUnitTargeted projectile2 = (Instantiate(passiveBasicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
            projectile2.transform.LookAt(target.transform);
            projectile2.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()));
            projectile2.OnProjectileUnitTargetedHit += PassiveBasicAttackHit;
        }

        isShootingPassiveShot = false;
    }

    protected override void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalAttack, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            passive.UseAbility(entityHit);
        }
        base.BasicAttackHit(basicAttackProjectile, entityHit, isACriticalAttack, willMiss);
    }

    private void PassiveBasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalAttack, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            passive.OnPassiveHit(entityHit, isACriticalAttack);
        }
        Destroy(basicAttackProjectile.gameObject);
    }
}
