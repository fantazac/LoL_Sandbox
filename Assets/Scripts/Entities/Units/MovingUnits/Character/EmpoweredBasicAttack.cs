using System.Collections;
using UnityEngine;

public abstract class EmpoweredBasicAttack : BasicAttack
{
    protected string empoweredBasicAttackPrefabPath;
    protected GameObject empoweredBasicAttackPrefab;

    protected Ability basicAttackEmpoweringAbility;
    protected IBasicAttackEmpoweringAbility iBasicAttackEmpoweringAbility;

    protected bool empoweringAbilityWasActiveOnBasicAttackCast;

    public virtual void SetBasicAttackEmpoweringAbility(Ability basicAttackEmpoweringAbility)
    {
        this.basicAttackEmpoweringAbility = basicAttackEmpoweringAbility;
        iBasicAttackEmpoweringAbility = (IBasicAttackEmpoweringAbility)basicAttackEmpoweringAbility;
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        empoweredBasicAttackPrefab = Resources.Load<GameObject>(empoweredBasicAttackPrefabPath);
    }

    protected override IEnumerator ShootBasicAttack(Unit target)
    {
        SetupBeforeAttackDelay(target);

        yield return delayAttack;

        SetupAfterAttackDelay(target, empoweringAbilityWasActiveOnBasicAttackCast ? empoweredBasicAttackPrefab : basicAttackPrefab);

        shootBasicAttackCoroutine = null;
    }

    protected void SetupBeforeAttackDelay(Unit target)
    {
        ((Champion)unit).OrientationManager.RotateCharacterUntilReachedTarget(target.transform, true, true);
        empoweringAbilityWasActiveOnBasicAttackCast = unit.BuffManager.IsAffectedByBuff(basicAttackEmpoweringAbility.AbilityBuffs[0]);
    }

    protected void SetupAfterAttackDelay(Unit target, GameObject basicAttackPrefab)
    {
        BasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Champion)unit).OrientationManager.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(affectedTeams, target, speed, AttackIsCritical.CheckIfAttackIsCritical(unit.StatsManager.CriticalStrikeChance.GetTotal()));
        if (empoweringAbilityWasActiveOnBasicAttackCast)
        {
            projectile.OnProjectileHit += BasicAttackHit; // The one in this class
        }
        else
        {
            projectile.OnProjectileHit += base.BasicAttackHit; // The one in the BasicAttack class
        }
    }

    protected override void BasicAttackHit(Projectile basicAttackProjectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(unit.StatusManager.IsBlinded() || willMiss))
        {
            EmpoweredBasicAttackHit(unitHit, isACriticalStrike);
        }

        base.BasicAttackHit(basicAttackProjectile, unitHit, isACriticalStrike, willMiss);
    }

    protected virtual void EmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike)
    {
        iBasicAttackEmpoweringAbility.OnEmpoweredBasicAttackHit(unitHit, isACriticalStrike);
    }
}
