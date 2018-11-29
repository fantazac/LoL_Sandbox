using UnityEngine;
using System.Collections;

public abstract class EntityBasicAttack : EntityDamageSource
{
    protected string basicAttackPrefabPath;
    protected GameObject basicAttackPrefab;

    protected float delayPercentBeforeAttack;//charging before actually launching/doing the attack
    protected WaitForSeconds delayAttack;

    protected IEnumerator shootBasicAttackCoroutine;

    protected Entity entity;

    protected Entity currentTarget;

    public bool AttackIsInQueue { get; protected set; }
    public EntityBasicAttackCycle EntityBasicAttackCycle { get; private set; }

    protected float speed;

    protected EntityBasicAttack()
    {
        damageType = DamageType.PHYSICAL;
    }

    protected void Awake()
    {
        EntityBasicAttackCycle = gameObject.AddComponent<EntityBasicAttackCycle>();
    }

    protected virtual void OnEnable()
    {
        entity = GetComponent<Entity>();

        ModifyValues();
    }

    protected virtual void Start()
    {
        LoadPrefabs();
    }

    protected virtual void LoadPrefabs()
    {
        basicAttackPrefab = Resources.Load<GameObject>(basicAttackPrefabPath);
    }

    protected void ModifyValues()
    {
        speed *= StaticObjects.MultiplyingFactor;
    }

    public virtual void ChangeAttackSpeedCycleDuration(float totalAttackSpeed, bool attackSpeedIncreased)
    {
        float attackSpeedCycleDuration = 1f / totalAttackSpeed;
        EntityBasicAttackCycle.SetAttackSpeedCycleDuration(attackSpeedCycleDuration * (1 - delayPercentBeforeAttack));
        delayAttack = new WaitForSeconds(attackSpeedCycleDuration * delayPercentBeforeAttack);
        if (attackSpeedIncreased && shootBasicAttackCoroutine != null && EntityBasicAttackCycle.AttackSpeedCycleIsReady)
        {
            ResetBasicAttack();
        }
    }

    public void SetupBasicAttack(Entity currentlySelectedTarget, bool setupEvent)
    {
        currentTarget = currentlySelectedTarget;
        if (setupEvent)
        {
            //TODO: EntityMovement will become the parent of CharacterMovement
            ((Character)entity).CharacterMovementManager.CharacterIsInTargetRange += UseBasicAttack;
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

    public void CancelCurrentBasicAttackToCastAbility()
    {
        if (AttackIsInQueue)
        {
            StopBasicAttack();//This is so CharacterAutoAttack doesn't shoot while an ability is active
            if (currentTarget != null)
            {
                ((Character)entity).CharacterMovementManager.SetMoveTowardsTarget(currentTarget, entity.EntityStatsManager.AttackRange.GetTotal(), true);
            }
        }
    }

    public Entity CurrentTarget()
    {
        return currentTarget;
    }

    public void ResetBasicAttack()
    {
        AttackIsInQueue = false;
        EntityBasicAttackCycle.ResetBasicAttack();
    }

    protected void Update()
    {
        if (currentTarget != null && ((Character)entity).CharacterMovementManager.GetBasicAttackTarget() != currentTarget && !AttackIsInQueue &&
            EntityBasicAttackCycle.AttackSpeedCycleIsReady && entity.EntityStatusManager.CanUseBasicAttacks() &&
            ((Character)entity).CharacterAbilityManager.CanUseBasicAttacks() && !entity.EntityDisplacementManager.IsBeingDisplaced)
        {
            StartBasicAttack();
        }
    }

    protected void StartBasicAttack()
    {
        AttackIsInQueue = true;
        ((Character)entity).CharacterMovementManager.SetMoveTowardsTarget(currentTarget, entity.EntityStatsManager.AttackRange.GetTotal(), true);
    }

    public void UseBasicAttackFromAutoAttackOrTaunt(Entity target)
    {
        UseBasicAttack(target);
        currentTarget = null;
    }

    protected virtual void UseBasicAttack(Entity target)
    {
        currentTarget = target;
        if (EntityBasicAttackCycle.AttackSpeedCycleIsReady)
        {
            AttackIsInQueue = true;
            if (shootBasicAttackCoroutine != null)
            {
                StopCoroutine(shootBasicAttackCoroutine);
            }
            shootBasicAttackCoroutine = ShootBasicAttack(target);
            StartCoroutine(shootBasicAttackCoroutine);
        }
    }

    protected virtual IEnumerator ShootBasicAttack(Entity target)
    {
        ((Character)entity).CharacterOrientationManager.RotateCharacterUntilReachedTarget(target.transform, true, true);

        yield return delayAttack;

        EntityBasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Character)entity).CharacterOrientationManager.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStatsManager.CriticalStrikeChance.GetTotal()), entity.EntityStatusManager.IsBlinded());
        projectile.OnAbilityEffectHit += BasicAttackHit;

        if (entity is Character)
        {
            ((Character)entity).CharacterOnAttackEffectsManager.ApplyOnAttackEffectsToEntityHit(target);
        }

        shootBasicAttackCoroutine = null;
    }

    protected virtual void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            ApplyDamageToEntityHit(entityHit, isACriticalStrike);
        }
        Destroy(basicAttackProjectile.gameObject);
    }

    protected virtual void ApplyDamageToEntityHit(Entity entityHit, bool isACriticalStrike)
    {
        float damage = GetBasicAttackDamage(entityHit, isACriticalStrike);
        DamageEntity(entityHit, damage);
        if (entity is Character)
        {
            ((Character)entity).CharacterOnHitEffectsManager.ApplyOnHitEffectsToEntityHit(entityHit, damage);
        }
        entityHit.EntityEffectSourceManager.EntityHitByBasicAttack(entity);
    }

    protected float GetBasicAttackDamage(Entity entityHit, bool isACriticalAttack)
    {
        //TODO: Right now, it considers the basic attack will ALWAYS do physical damage. Will need to implement damage type check (ex. corki autos deal 80% magic, 20% physical)
        return ApplyDamageModifiers(entityHit, entity.EntityStatsManager.AttackDamage.GetTotal()) *
            (isACriticalAttack ? entity.EntityStatsManager.CriticalStrikeDamage.GetTotal() * (1f - entityHit.EntityStatsManager.CriticalStrikeDamageReduction.GetTotal()) : 1f);
    }

    protected float ApplyDamageModifiers(Entity entityHit, float damage)
    {
        float totalResistance = entityHit.EntityStatsManager.Armor.GetTotal();
        totalResistance *= (1 - entity.EntityStatsManager.ArmorPenetrationPercent.GetTotal());
        totalResistance -= entity.EntityStatsManager.Lethality.GetCurrentValue();
        return damage * GetResistanceDamageReceivedModifier(totalResistance) * entityHit.EntityStatsManager.PhysicalDamageReceivedModifier.GetTotal() * entity.EntityStatsManager.PhysicalDamageModifier.GetTotal();
    }

    protected float GetResistanceDamageReceivedModifier(float totalResistance)
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
