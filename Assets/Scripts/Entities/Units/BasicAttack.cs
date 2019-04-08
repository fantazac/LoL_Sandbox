using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasicAttack : DamageSource
{
    protected string basicAttackPrefabPath;
    protected GameObject basicAttackPrefab;

    protected float delayPercentBeforeAttack; //charging before actually launching/doing the attack
    protected WaitForSeconds delayAttack;

    protected IEnumerator shootBasicAttackCoroutine;

    protected Unit unit;
    private Champion champion; //TODO: Change this

    protected Unit currentTarget;

    public bool AttackIsInQueue { get; protected set; }
    public BasicAttackCycle BasicAttackCycle { get; private set; }

    protected float speed;

    protected List<Team> affectedTeams;

    protected BasicAttack()
    {
        damageType = DamageType.PHYSICAL;
    }

    protected void Awake()
    {
        BasicAttackCycle = gameObject.AddComponent<BasicAttackCycle>();
    }

    protected void OnEnable()
    {
        unit = GetComponent<Unit>();
        if (unit is Champion c)
        {
            champion = c;
        }

        ModifyValues();
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected void Start()
    {
        LoadPrefabs();
    }

    protected virtual void LoadPrefabs()
    {
        basicAttackPrefab = Resources.Load<GameObject>(basicAttackPrefabPath);
    }

    private void ModifyValues()
    {
        speed *= StaticObjects.MultiplyingFactor;
    }

    public void ChangeAttackSpeedCycleDuration(float totalAttackSpeed, bool attackSpeedIncreased)
    {
        float attackSpeedCycleDuration = 1f / totalAttackSpeed;
        BasicAttackCycle.SetAttackSpeedCycleDuration(attackSpeedCycleDuration * (1 - delayPercentBeforeAttack));
        delayAttack = new WaitForSeconds(attackSpeedCycleDuration * delayPercentBeforeAttack);
        if (attackSpeedIncreased && shootBasicAttackCoroutine != null && BasicAttackCycle.AttackSpeedCycleIsReady)
        {
            ResetBasicAttack();
        }
    }

    public void SetupBasicAttack(Unit currentlySelectedTarget, bool setupEvent)
    {
        currentTarget = currentlySelectedTarget;
        if (setupEvent && champion)
        {
            //TODO: all units will have a movement manager
            champion.ChampionMovementManager.ChampionIsInTargetRange += UseBasicAttack;
        }
    }

    public virtual void StopBasicAttack(bool isCrowdControlled = false)
    {
        AttackIsInQueue = false;
        if (shootBasicAttackCoroutine != null)
        {
            StopCoroutine(shootBasicAttackCoroutine);
            shootBasicAttackCoroutine = null;
        }

        if (!currentTarget) return;

        if (isCrowdControlled)
        {
            StartBasicAttack();
        }
        else
        {
            currentTarget = null;
        }
    }

    public void CancelCurrentBasicAttackToCastAbility()
    {
        if (!AttackIsInQueue) return;

        StopBasicAttack(); //This is so CharacterAutoAttack doesn't shoot while an ability is active
        if (currentTarget && champion)
        {
            champion.ChampionMovementManager.SetMoveTowardsTarget(currentTarget, unit.StatsManager.AttackRange.GetTotal(), true);
        }
    }

    public Unit CurrentTarget()
    {
        return currentTarget;
    }

    public void ResetBasicAttack()
    {
        AttackIsInQueue = false;
        BasicAttackCycle.ResetBasicAttack();
    }

    protected void Update()
    {
        if (!champion) return;

        if (currentTarget && champion.ChampionMovementManager.GetBasicAttackTarget() != currentTarget && !AttackIsInQueue &&
            BasicAttackCycle.AttackSpeedCycleIsReady && unit.StatusManager.CanUseBasicAttacks() &&
            champion.AbilityManager.CanUseBasicAttacks() && !unit.DisplacementManager.IsBeingDisplaced)
        {
            StartBasicAttack();
        }
    }

    protected void StartBasicAttack()
    {
        AttackIsInQueue = true;
        champion.ChampionMovementManager.SetMoveTowardsTarget(currentTarget, unit.StatsManager.AttackRange.GetTotal(), true);
    }

    public void UseBasicAttackFromAutoAttackOrTaunt(Unit target)
    {
        UseBasicAttack(target);
        currentTarget = null;
    }

    protected virtual void UseBasicAttack(Unit target)
    {
        currentTarget = target;

        if (!BasicAttackCycle.AttackSpeedCycleIsReady) return;

        AttackIsInQueue = true;
        if (shootBasicAttackCoroutine != null)
        {
            StopCoroutine(shootBasicAttackCoroutine);
        }

        shootBasicAttackCoroutine = ShootBasicAttack(target);
        StartCoroutine(shootBasicAttackCoroutine);
    }

    protected virtual IEnumerator ShootBasicAttack(Unit target)
    {
        champion.OrientationManager.RotateCharacterUntilReachedTarget(target.transform, true, true);

        yield return delayAttack;

        BasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        champion.OrientationManager.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(affectedTeams, target, speed, AttackIsCritical.CheckIfAttackIsCritical(unit.StatsManager.CriticalStrikeChance.GetTotal()),
            unit.StatusManager.IsBlinded());
        projectile.OnProjectileHit += BasicAttackHit;

        champion.OnAttackEffectsManager.ApplyOnAttackEffectsToUnitHit(target);

        shootBasicAttackCoroutine = null;
    }

    protected virtual void BasicAttackHit(Projectile basicAttackProjectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (!unit.StatusManager.IsBlinded() && !willMiss)
        {
            ApplyDamageToUnitHit(unitHit, isACriticalStrike);
        }

        Destroy(basicAttackProjectile.gameObject);
    }

    private void ApplyDamageToUnitHit(Unit unitHit, bool isACriticalStrike)
    {
        float damage = GetBasicAttackDamage(unitHit, isACriticalStrike);
        DamageUnit(unitHit, damage);
        champion.OnHitEffectsManager.ApplyOnHitEffectsToUnitHit(unitHit, damage);
        unitHit.EffectSourceManager.UnitHitByBasicAttack(unit);
    }

    private float GetBasicAttackDamage(Unit unitHit, bool isACriticalAttack)
    {
        //TODO: Right now, it considers the basic attack will ALWAYS do physical damage. Will need to implement damage type check (ex. corki autos deal 80% magic, 20% physical)
        return ApplyDamageModifiers(unitHit, unit.StatsManager.AttackDamage.GetTotal()) *
               (isACriticalAttack ? unit.StatsManager.CriticalStrikeDamage.GetTotal() * (1 - unitHit.StatsManager.CriticalStrikeDamageReduction.GetTotal()) : 1);
    }

    private float ApplyDamageModifiers(Unit unitHit, float damage)
    {
        float totalResistance = unitHit.StatsManager.Armor.GetTotal();
        totalResistance *= (1 - unit.StatsManager.ArmorPenetrationPercent.GetTotal());
        totalResistance -= unit.StatsManager.Lethality.GetCurrentValue();
        return damage * GetResistanceDamageReceivedModifier(totalResistance) * unitHit.StatsManager.PhysicalDamageReceivedModifier.GetTotal() *
               unit.StatsManager.PhysicalDamageModifier.GetTotal();
    }

    private float GetResistanceDamageReceivedModifier(float totalResistance)
    {
        if (totalResistance >= 0)
        {
            return 100 / (100 + totalResistance);
        }
        else
        {
            return 2 - (100 / (100 - totalResistance));
        }
    }
}
