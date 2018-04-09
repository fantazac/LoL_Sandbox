using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucianBasicAttack : CharacterBasicAttack
{
    [SerializeField]
    private GameObject passiveBasicAttackPrefab;

    private WaitForSeconds delayPassiveShot;

    private Lucian_P passive;

    private bool isShootingPassiveShot;
    private bool passiveWasActiveOnBasicAttackCast;

    protected LucianBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 2500;

        delayPassiveShot = new WaitForSeconds(0.2f);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        passive = GetComponent<Lucian_P>();
    }

    public override void StopBasicAttack()
    {
        currentTarget = null;
        attackIsInQueue = false;
        if (!isShootingPassiveShot)
        {
            StopAllCoroutines();
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
        passiveWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(passive) != null;

        yield return delayAttack;

        isShootingPassiveShot = true;

        if (!passiveWasActiveOnBasicAttackCast)
        {
            passiveWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(passive) != null;
        }

        entity.EntityBasicAttackCycle.LockBasicAttack();
        attackIsInQueue = false;

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed);
        if (passiveWasActiveOnBasicAttackCast)
        {
            projectile.OnAbilityEffectHit += BasicAttackHit;
        }
        else
        {
            projectile.OnAbilityEffectHit += base.BasicAttackHit;
        }

        if (passiveWasActiveOnBasicAttackCast)
        {
            passiveWasActiveOnBasicAttackCast = false;
            passive.ConsumeBuff(entity);

            yield return delayPassiveShot;

            ProjectileUnitTargeted projectile2 = (Instantiate(passiveBasicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
            projectile2.transform.LookAt(target.transform);
            projectile2.ShootProjectile(entity.Team, target, speed);
            projectile2.OnAbilityEffectHit += PassiveBasicAttackHit;
        }

        isShootingPassiveShot = false;
    }

    protected override void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit)
    {
        passive.UseAbility(entityHit);
        base.BasicAttackHit(basicAttackProjectile, entityHit);
    }

    private void PassiveBasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit)
    {
        passive.OnPassiveHit(entityHit);
        Destroy(basicAttackProjectile.gameObject);
        CallOnBasicAttackHitEvent(entityHit);
    }
}
