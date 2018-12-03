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

    protected override IEnumerator ShootBasicAttack(Unit target)
    {
        SetupBeforeAttackDelay(target);

        yield return delayAttack;

        SetupAfterAttackDelay(target, empoweringAbilityWasActiveOnBasicAttackCast ? empoweredBasicAttackPrefab : basicAttackPrefab);

        shootBasicAttackCoroutine = null;
    }

    protected void SetupBeforeAttackDelay(Unit target)
    {
        ((Character)unit).CharacterOrientationManager.RotateCharacterUntilReachedTarget(target.transform, true, true);
        empoweringAbilityWasActiveOnBasicAttackCast = unit.BuffManager.IsAffectedByBuff(basicAttackEmpoweringAbility.AbilityBuffs[0]);
    }

    protected void SetupAfterAttackDelay(Unit target, GameObject basicAttackPrefab)
    {
        BasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Character)unit).CharacterOrientationManager.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(unit.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(unit.StatsManager.CriticalStrikeChance.GetTotal()));
        if (empoweringAbilityWasActiveOnBasicAttackCast)
        {
            projectile.OnAbilityEffectHit += BasicAttackHit;
        }
        else
        {
            projectile.OnAbilityEffectHit += base.BasicAttackHit;
        }
    }

    protected override void BasicAttackHit(AbilityEffect basicAttackProjectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(unit.StatusManager.IsBlinded() || willMiss))
        {
            OnEmpoweredBasicAttackHit(unitHit, isACriticalStrike);
        }
        base.BasicAttackHit(basicAttackProjectile, unitHit, isACriticalStrike, willMiss);
    }

    protected virtual void OnEmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike)
    {
        basicAttackEmpoweringAbility.OnEmpoweredBasicAttackHit(unitHit, isACriticalStrike);
    }
}
