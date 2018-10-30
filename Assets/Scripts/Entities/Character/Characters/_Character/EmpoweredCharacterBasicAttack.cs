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
        ((Character)entity).CharacterOrientationManager.RotateCharacterUntilReachedTarget(target.transform, true, true);
        empoweringAbilityWasActiveOnBasicAttackCast = entity.EntityBuffManager.GetBuff(basicAttackEmpoweringAbility.AbilityBuffs[0]) != null;
    }

    protected void SetupAfterAttackDelay(Entity target, GameObject basicAttackPrefab)
    {
        entity.EntityBasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Character)entity).CharacterOrientationManager.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()));
        if (empoweringAbilityWasActiveOnBasicAttackCast)
        {
            projectile.OnAbilityEffectHit += BasicAttackHit;
        }
        else
        {
            projectile.OnAbilityEffectHit += base.BasicAttackHit;
        }
    }

    protected override void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            OnEmpoweredBasicAttackHit(entityHit, isACriticalStrike);
        }
        base.BasicAttackHit(basicAttackProjectile, entityHit, isACriticalStrike, willMiss);
    }

    protected virtual void OnEmpoweredBasicAttackHit(Entity entityHit, bool isACriticalStrike)
    {
        basicAttackEmpoweringAbility.OnEmpoweredBasicAttackHit(entityHit, isACriticalStrike);
    }
}
