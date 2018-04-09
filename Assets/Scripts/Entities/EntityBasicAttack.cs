using UnityEngine;
using System.Collections;

public abstract class EntityBasicAttack : MonoBehaviour
{
    [SerializeField]
    protected GameObject basicAttackPrefab;

    protected float delayPercentBeforeAttack;//charging before actually launching/doing the attack
    protected WaitForSeconds delayAttack;

    protected Entity entity;

    protected Entity currentTarget;

    protected bool attackIsInQueue;

    protected float speed;

    public delegate void OnBasicAttackHitHandler(Entity entityHit);
    public event OnBasicAttackHitHandler OnBasicAttackHit;

    protected virtual void OnEnable()
    {
        entity = GetComponent<Entity>();

        ModifyValues();
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
        ((Character)entity).CharacterMovement.CharacterIsInRange += UseBasicAttack;
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
        projectile.ShootProjectile(entity.Team, target, speed);
        projectile.OnAbilityEffectHit += BasicAttackHit;
    }

    protected virtual void BasicAttackHit(AbilityEffect basicAttackProjectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(entity.EntityStats.AttackDamage.GetTotal());
        Destroy(basicAttackProjectile.gameObject);
        CallOnBasicAttackHitEvent(entityHit);
    }

    protected void CallOnBasicAttackHitEvent(Entity entityHit)
    {
        if (OnBasicAttackHit != null)
        {
            OnBasicAttackHit(entityHit);
        }
    }
}
