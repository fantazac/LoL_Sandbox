using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    protected float damage;
    protected float duration;

    protected bool collidersInChildren;

    public List<Health> HealthOfUnitsAlreadyHitWithProjectile { get; protected set; }

    public void ActivateAreaOfEffect(List<Health> healthOfUnitsAlreadyHitWithProjectile, float damage, float duration)
    {
        this.HealthOfUnitsAlreadyHitWithProjectile = healthOfUnitsAlreadyHitWithProjectile;
        this.damage = damage;
        this.duration = duration;
        StartCoroutine(ActivateArea());
    }

    public void ActivateAreaOfEffect(List<Health> healthOfUnitsAlreadyHitWithProjectile, float damage, float duration, bool collidersInChildren)
    {
        if (collidersInChildren)
        {
            this.collidersInChildren = collidersInChildren;
            foreach(AreaOfEffectCollider aoeCollider in GetComponentsInChildren<AreaOfEffectCollider>())
            {
                aoeCollider.OnTriggerEnterInChild += OnTriggerEnterInChild;
            }
        }
        ActivateAreaOfEffect(healthOfUnitsAlreadyHitWithProjectile, damage, duration);
    }

    protected IEnumerator ActivateArea()
    {
        float timeBeforeFrame = Time.deltaTime;

        yield return null;

        DisableColliders();

        yield return new WaitForSeconds(duration - (Time.deltaTime - timeBeforeFrame));

        Destroy(gameObject);
    }

    protected void DisableColliders()
    {
        if (collidersInChildren)
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    protected void OnTriggerEnterInChild(Collider collider)
    {
        Health targetHealth = collider.gameObject.GetComponent<Health>();

        if (targetHealth != null && CanHitTarget(targetHealth))
        {
            HealthOfUnitsAlreadyHitWithProjectile.Add(targetHealth);
            targetHealth.Hit(damage);
        }
    }

    protected bool CanHitTarget(Health targetHealth)
    {
        foreach (Health health in HealthOfUnitsAlreadyHitWithProjectile)
        {
            if (health == targetHealth)
            {
                return false;
            }
        }

        return true;
    }
}
