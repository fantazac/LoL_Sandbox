using System.Collections;
using UnityEngine;

public abstract class Projectile : AbilityEffect
{
    protected float speed;
    protected float range;
    protected bool isACriticalStrike;
    protected bool willMiss;

    protected Vector3 initialPosition;

    public delegate void OnProjectileReachedEndHandler(Projectile projectile);
    public event OnProjectileReachedEndHandler OnProjectileReachedEnd;

    public void ShootProjectile(EntityTeam teamOfCallingEntity, AbilityAffectedUnitType affectedUnitType, float speed, float range, bool isACriticalStrike = false, bool willMiss = false)
    {
        this.teamOfCallingEntity = teamOfCallingEntity;
        this.affectedUnitType = affectedUnitType;
        this.speed = speed;
        this.range = range;
        this.isACriticalStrike = isACriticalStrike;
        this.willMiss = willMiss;
        initialPosition = transform.position;
        StartCoroutine(ActivateAbilityEffect());
    }

    protected void OnProjectileReachedEndOfRange()
    {
        if (OnProjectileReachedEnd != null)
        {
            OnProjectileReachedEnd(this);
        }
    }

    public virtual void ProjectileEffect() { }

    protected override IEnumerator ActivateAbilityEffect()
    {
        while (Vector3.Distance(transform.position, initialPosition) < range)
        {
            transform.position += transform.forward * Time.deltaTime * speed;

            yield return null;
        }

        OnProjectileReachedEndOfRange();
    }
}
