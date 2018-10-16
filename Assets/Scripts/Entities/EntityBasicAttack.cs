using UnityEngine;
using System.Collections;

public abstract class EntityBasicAttack : MonoBehaviour
{
    protected string basicAttackPrefabPath;
    protected GameObject basicAttackPrefab;

    protected float delayPercentBeforeAttack;//charging before actually launching/doing the attack
    protected WaitForSeconds delayAttack;

    protected IEnumerator shootBasicAttackCoroutine;

    protected Entity entity;

    protected Entity currentTarget;

    public bool AttackIsInQueue { get; protected set; }

    protected float speed;

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

    public virtual void ChangeAttackSpeedCycleDuration(float totalAttackSpeed)
    {
        float attackSpeedCycleDuration = 1f / totalAttackSpeed;
        entity.EntityBasicAttackCycle.SetAttackSpeedCycleDuration(attackSpeedCycleDuration * (1 - delayPercentBeforeAttack));
        delayAttack = new WaitForSeconds(attackSpeedCycleDuration * delayPercentBeforeAttack);
    }

    public void SetupBasicAttack(Entity currentlySelectedTarget, bool setupEvent)
    {
        currentTarget = currentlySelectedTarget;
        if (setupEvent)
        {
            //TODO: EntityMovement will become the parent of CharacterMovement
            ((Character)entity).CharacterMovement.CharacterIsInTargetRange += UseBasicAttack;
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

    public Entity CurrentTarget()
    {
        return currentTarget;
    }

    public void ResetBasicAttack()
    {
        AttackIsInQueue = false;
        entity.EntityBasicAttackCycle.ResetBasicAttack();
    }

    protected void Update()
    {
        if (currentTarget != null && ((Character)entity).CharacterMovement.GetBasicAttackTarget() != currentTarget && !AttackIsInQueue && entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady &&
            !((Character)entity).CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks())
        {
            StartBasicAttack();
        }
    }

    protected void StartBasicAttack()
    {
        AttackIsInQueue = true;
        ((Character)entity).CharacterMovement.SetMoveTowardsTarget(currentTarget, entity.EntityStats.AttackRange.GetTotal(), true);
    }

    public void UseBasicAttackFromAutoAttack(Entity target)
    {
        UseBasicAttack(target);
        currentTarget = null;
    }

    protected virtual void UseBasicAttack(Entity target)
    {
        currentTarget = target;
        if (entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
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
        ((Character)entity).CharacterOrientation.RotateCharacterUntilReachedTarget(target.transform, true, true);

        yield return delayAttack;

        entity.EntityBasicAttackCycle.LockBasicAttack();
        AttackIsInQueue = false;
        ((Character)entity).CharacterOrientation.StopTargetRotation();

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()), entity.EntityStatusManager.IsBlinded());
        projectile.OnAbilityEffectHit += BasicAttackHit;

        shootBasicAttackCoroutine = null;
    }

    protected virtual void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            float damage = GetBasicAttackDamage(entityHit, isACriticalStrike);
            entityHit.EntityStats.Health.Reduce(damage);
            if (entity is Character)
            {
                ((Character)entity).CharacterOnHitEffectsManager.ApplyOnHitEffectsToEntityHit(entityHit, damage);
                ((Character)entity).CharacterOnAttackEffectsManager.ApplyOnAttackEffectsToEntityHit(entityHit, damage);
            }
            entityHit.EntityEffectSourceManager.EntityHitByBasicAttack(entity);
        }
        Destroy(basicAttackProjectile.gameObject);
    }

    protected float GetBasicAttackDamage(Entity entityHit, bool IsACriticalAttack)
    {
        return ApplyDamageModifiers(entityHit, entity.EntityStats.AttackDamage.GetTotal()) *
            (IsACriticalAttack ? entity.EntityStats.CriticalStrikeDamage.GetTotal() * (1f - entityHit.EntityStats.CriticalStrikeDamageReduction.GetTotal()) : 1f);
    }

    protected float ApplyDamageModifiers(Entity entityHit, float damage)
    {
        float totalResistance = entityHit.EntityStats.Armor.GetTotal();
        totalResistance *= (1 - entity.EntityStats.ArmorPenetrationPercent.GetTotal());
        totalResistance -= entity.EntityStats.Lethality.GetCurrentValue();
        return damage * GetResistanceDamageReceivedModifier(totalResistance) * entity.EntityStats.PhysicalDamageIncreaseModifier.GetTotal();
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
