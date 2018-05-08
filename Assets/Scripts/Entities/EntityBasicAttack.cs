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

    protected bool attackIsInQueue;

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

    public void SetupBasicAttack()
    {
        //TODO: EntityMovement will become the parent of CharacterMovement
        ((Character)entity).CharacterMovement.CharacterIsInTargetRange += UseBasicAttack;
    }

    public virtual void StopBasicAttack()
    {
        currentTarget = null;
        attackIsInQueue = false;
        StopAllCoroutines();
    }

    public bool AttackIsInQueue()
    {
        return attackIsInQueue;
    }

    public Entity CurrentTarget()
    {
        return currentTarget;
    }

    public void ResetBasicAttack()
    {
        attackIsInQueue = false;
        entity.EntityBasicAttackCycle.ResetBasicAttack();
    }

    protected void Update()
    {
        if (currentTarget != null && !attackIsInQueue && entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
        {
            attackIsInQueue = true;
            ((Character)entity).CharacterMovement.SetMoveTowardsTarget(currentTarget, entity.EntityStats.AttackRange.GetTotal(), true);
        }
    }

    protected virtual void UseBasicAttack(Entity target)
    {
        currentTarget = target;
        if (entity.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
        {
            attackIsInQueue = true;
            StopAllCoroutines();
            StartCoroutine(ShootBasicAttack(target));
        }
    }

    protected virtual IEnumerator ShootBasicAttack(Entity target)
    {
        yield return delayAttack;

        entity.EntityBasicAttackCycle.LockBasicAttack();
        attackIsInQueue = false;

        ProjectileUnitTargeted projectile = (Instantiate(basicAttackPrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
        projectile.ShootProjectile(entity.Team, target, speed, AttackIsCritical.CheckIfAttackIsCritical(entity.EntityStats.CriticalStrikeChance.GetTotal()));
        projectile.OnProjectileUnitTargetedHit += BasicAttackHit;
    }

    protected virtual void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit, bool isACriticalAttack)
    {
        float damage = GetBasicAttackDamage(entityHit);
        if (isACriticalAttack)
        {
            damage *= 2;//TODO: Crit reduction (randuins)? Crit multiplier different than +100% (Jhin, IE)?
        }
        entityHit.EntityStats.Health.Reduce(damage);
        Destroy(basicAttackProjectile.gameObject);
        if (entity is Character)
        {
            ((Character)entity).CharacterOnHitEffectsManager.ApplyOnHitEffectsToEntityHit(entityHit, damage);
        }
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
