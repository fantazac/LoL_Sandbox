using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected float speed;
    protected float range;
    protected float damage;

    protected bool destroyProjectile;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected Vector3 initialPosition;

    protected List<Health> healthOfUnitsAlreadyHitWithProjectile;//CHANGE TYPE, TEMPORAIRE

    protected Projectile()
    {
        healthOfUnitsAlreadyHitWithProjectile = new List<Health>();
    }

    public void ShootProjectile(Health healthOfShooter, float speed, float range, float damage)
    {
        healthOfUnitsAlreadyHitWithProjectile.Add(healthOfShooter);//TEMPORAIRE
        this.speed = speed;
        this.range = range;
        this.damage = damage;
        initialPosition = transform.position;
        StartCoroutine(Shoot());
    }

    protected virtual IEnumerator Shoot() { yield return null; }

    protected virtual void OnTriggerEnter(Collider collider) { }

    protected virtual bool CanHitTarget(Health targetHealth) { return false; }
}
