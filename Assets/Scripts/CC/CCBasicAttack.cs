using System.Collections;
using UnityEngine;

public class CCBasicAttack : CharacterBasicAttack
{
    private string ccQBasicAttackPrefabPath;
    private GameObject ccQBasicAttackPrefab;

    private CC_Q ccQ;

    private bool qWasActiveOnBasicAttackCast;

    protected CCBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1800;

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/CC/CCBA";
        ccQBasicAttackPrefabPath = "BasicAttacksPrefabs/Characters/CC/CCBAQ";
    }

    public void SetQ(CC_Q ccQ)
    {
        this.ccQ = ccQ;
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        ccQBasicAttackPrefab = Resources.Load<GameObject>(ccQBasicAttackPrefabPath);
    }

    protected override IEnumerator ShootBasicAttack(Entity target)
    {
        ((Character)entity).CharacterOrientation.RotateCharacterUntilReachedTarget(target.transform, true, true);
        qWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(ccQ.AbilityBuffs[0]) != null;

        yield return delayAttack;

        entity.EntityBasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Character)entity).CharacterOrientation.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(qWasActiveOnBasicAttackCast ? ccQBasicAttackPrefab : basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()));
        if (qWasActiveOnBasicAttackCast)
        {
            projectile.OnProjectileUnitTargetedHit += BasicAttackHit;
        }
        else
        {
            projectile.OnProjectileUnitTargetedHit += base.BasicAttackHit;
        }

        shootBasicAttackCoroutine = null;
    }

    protected override void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalAttack, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            ccQ.OnQHit(entityHit);
        }
        base.BasicAttackHit(basicAttackProjectile, entityHit, isACriticalAttack, willMiss);
    }
}

