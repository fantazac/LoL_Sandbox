using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected float speed;
    protected float range;
    protected float damage;
    protected bool canHitAllies;
    protected EntityTeam teamOfShooter;

    protected bool alreadyHitTarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected Vector3 initialPosition;

    public List<Health> HealthOfUnitsAlreadyHit { get; protected set; }

    public delegate void OnProjectileHitHandler(Projectile projectile);
    public event OnProjectileHitHandler OnProjectileHit;

    public delegate void OnProjectileReachedEndHandler(Projectile projectile);
    public event OnProjectileReachedEndHandler OnProjectileReachedEnd;

    protected Projectile()
    {
        HealthOfUnitsAlreadyHit = new List<Health>();
    }

    public void ShootProjectile(EntityTeam teamOfShooter, float speed, float range, float damage)
    {
        this.teamOfShooter = teamOfShooter;
        this.speed = speed;
        this.range = range;
        this.damage = damage;
        initialPosition = transform.position;
        StartCoroutine(Shoot());
    }

    public void ShootProjectile(EntityTeam teamOfShooter, float speed, float range, float damage, bool canHitAllies)
    {
        this.canHitAllies = canHitAllies;
        ShootProjectile(teamOfShooter, speed, range, damage);
    }

    protected virtual IEnumerator Shoot() { yield return null; }

    protected virtual void OnTriggerEnter(Collider collider) { }

    protected virtual bool CanHitTarget(Health targetHealth) { return false; }

    protected void OnProjectileHitTarget()
    {
        if (OnProjectileHit != null)
        {
            OnProjectileHit(this);
        }
    }

    protected void OnProjectileReachedEndOfRange()
    {
        if (OnProjectileReachedEnd != null)
        {
            OnProjectileReachedEnd(this);
        }
    }
}
