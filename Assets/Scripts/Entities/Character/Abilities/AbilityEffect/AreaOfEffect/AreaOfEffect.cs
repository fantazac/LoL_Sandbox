using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    protected float damage;
    protected float duration;
    protected bool canHitAllies;
    protected EntityTeam teamOfShooter;

    protected bool collidersInChildren;

    public List<Health> HealthOfUnitsAlreadyHitWithProjectile { get; protected set; }

    public void ActivateAreaOfEffect(List<Health> healthOfUnitsAlreadyHitWithProjectile, EntityTeam teamOfShooter, bool canHitAllies, float damage, float duration)
    {
        this.HealthOfUnitsAlreadyHitWithProjectile = healthOfUnitsAlreadyHitWithProjectile;
        this.teamOfShooter = teamOfShooter;
        this.damage = damage;
        this.duration = duration;
        this.canHitAllies = canHitAllies;
        StartCoroutine(ActivateArea());
    }

    public void ActivateAreaOfEffect(List<Health> healthOfUnitsAlreadyHitWithProjectile, EntityTeam teamOfShooter, bool canHitAllies, float damage, float duration, bool collidersInChildren)
    {
        if (collidersInChildren)
        {
            this.collidersInChildren = collidersInChildren;
            foreach(AreaOfEffectCollider aoeCollider in GetComponentsInChildren<AreaOfEffectCollider>())
            {
                aoeCollider.OnTriggerEnterInChild += OnTriggerEnterInChild;
            }
        }
        ActivateAreaOfEffect(healthOfUnitsAlreadyHitWithProjectile, teamOfShooter, canHitAllies, damage, duration);
    }

    protected IEnumerator ActivateArea()
    {
        float timeBeforeFrame = Time.deltaTime;

        yield return null;//2 frames
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

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Character>().team != teamOfShooter || canHitAllies)
        {
            Health targetHealth = collider.gameObject.GetComponent<Health>();

            if (targetHealth != null && CanHitTarget(targetHealth))
            {
                HealthOfUnitsAlreadyHitWithProjectile.Add(targetHealth);
                targetHealth.Hit(damage);
            }
        }
    }

    protected void OnTriggerEnterInChild(Collider collider)
    {
        OnTriggerEnter(collider);
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
