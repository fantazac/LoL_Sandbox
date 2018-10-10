using System.Collections;
using UnityEngine;

public abstract class EmpoweredCharacterBasicAttack : CharacterBasicAttack
{
    protected string empoweredBasicAttackPrefabPath;
    protected GameObject empoweredBasicAttackPrefab;

    protected Ability basicAttackEmpoweringAbility;

    protected bool empoweringAbilityWasActiveOnBasicAttackCast;

    public void SetBasicAttackEmpoweringAbility(Ability basicAttackEmpoweringAbility)
    {
        this.basicAttackEmpoweringAbility = basicAttackEmpoweringAbility;
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        empoweredBasicAttackPrefab = Resources.Load<GameObject>(empoweredBasicAttackPrefabPath);
    }

    protected override IEnumerator ShootBasicAttack(Entity target)
    {
        SetupBeforeAttackDelay(target);

        yield return delayAttack;

        SetupAfterAttackDelay(target, empoweringAbilityWasActiveOnBasicAttackCast ? empoweredBasicAttackPrefab : basicAttackPrefab);

        shootBasicAttackCoroutine = null;
    }

    protected void SetupBeforeAttackDelay(Entity target)
    {
        ((Character)entity).CharacterOrientation.RotateCharacterUntilReachedTarget(target.transform, true, true);
        empoweringAbilityWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(basicAttackEmpoweringAbility.AbilityBuffs[0]) != null;
    }

    protected void SetupAfterAttackDelay(Entity target, GameObject basicAttackPrefab)
    {
        entity.EntityBasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Character)entity).CharacterOrientation.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()));
        if (empoweringAbilityWasActiveOnBasicAttackCast)
        {
            projectile.OnProjectileUnitTargetedHit += BasicAttackHit;
        }
        else
        {
            projectile.OnProjectileUnitTargetedHit += base.BasicAttackHit;
        }
    }

    protected override void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalAttack, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            OnEmpoweredBasicAttackHit(entityHit, isACriticalAttack);
        }
        base.BasicAttackHit(basicAttackProjectile, entityHit, isACriticalAttack, willMiss);
    }

    protected virtual void OnEmpoweredBasicAttackHit(Entity entityHit, bool isACriticalAttack)
    {
        basicAttackEmpoweringAbility.OnEmpoweredBasicAttackHit(entityHit, isACriticalAttack);
    }
}
