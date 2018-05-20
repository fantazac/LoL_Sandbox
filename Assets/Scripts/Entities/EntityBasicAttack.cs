using UnityEngine;
using System.Collections;

public abstract class EntityBasicAttack : MonoBehaviour
{
    protected string basicAttackPrefabPath;
    protected GameObject basicAttackPrefab;

    protected float delayPercentBeforeAttack;//charging before actually launching/doing the attack
    protected WaitForSeconds delayAttack;

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
        StopAllCoroutines();
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
        if (currentTarget != null && !AttackIsInQueue && entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady && 
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
            StopAllCoroutines();
            StartCoroutine(ShootBasicAttack(target));
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
        projectile.OnProjectileUnitTargetedHit += BasicAttackHit;
    }

    protected virtual void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalAttack, bool willMiss)
    {
        if (!(entity.EntityStatusManager.IsBlinded() || willMiss))
        {
            float damage = GetBasicAttackDamage(entityHit);
            if (isACriticalAttack)
            {
                damage *= 2;//TODO: Crit reduction (randuins)? Crit multiplier different than +100% (Jhin, IE)?
            }
            entityHit.EntityStats.Health.Reduce(damage);
            if (entity is Character)
            {
                ((Character)entity).CharacterOnHitEffectsManager.ApplyOnHitEffectsToEntityHit(entityHit, damage);
            }
        }
        Destroy(basicAttackProjectile.gameObject);
    }

    protected float GetBasicAttackDamage(Entity entityHit)
    {
        return ApplyResistanceToDamage(entityHit, entity.EntityStats.AttackDamage.GetTotal());
    }

    protected float ApplyResistanceToDamage(Entity entityHit, float damage)
    {
        float totalResistance = entityHit.EntityStats.Armor.GetTotal();
        totalResistance *= (1 - entity.EntityStats.ArmorPenetrationPercent.GetTotal());
        totalResistance -= entity.EntityStats.Lethality.GetCurrentValue();
        return damage * GetResistanceDamageTakenMultiplier(totalResistance);
    }

    protected float GetResistanceDamageTakenMultiplier(float totalResistance)
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
