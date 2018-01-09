using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileMovement : MonoBehaviour
{
    private float speed;
    private float range;
    private float damage;
    private bool canPassThroughUnits;
    private bool destroyProjectile;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    private Vector3 initialPosition;

    private List<Health> healthOfUnitsAlreadyHitWithProjectile;//CHANGE TYPE, TEMPORAIRE

    private ProjectileMovement()
    {
        healthOfUnitsAlreadyHitWithProjectile = new List<Health>();
    }

    public void ShootProjectile(Health healthOfShooter, float speed, float range, float damage, bool canPassThroughUnits = false)
    {
        healthOfUnitsAlreadyHitWithProjectile.Add(healthOfShooter);//TEMPORAIRE
        this.speed = speed;
        this.range = range;
        this.damage = damage;
        this.canPassThroughUnits = canPassThroughUnits;
        initialPosition = transform.position;
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (Vector3.Distance(transform.position, initialPosition) < range)
        {
            transform.position += transform.forward * Time.deltaTime * speed;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!destroyProjectile)
        {
            Health targetHealth = collider.gameObject.GetComponent<Health>();

            if (targetHealth != null && CanHitTarget(targetHealth))
            {
                healthOfUnitsAlreadyHitWithProjectile.Add(targetHealth);
                targetHealth.Hit(damage);
                if (!canPassThroughUnits)
                {
                    destroyProjectile = true;
                    Destroy(gameObject);
                }
            }
        }
    }

    private bool CanHitTarget(Health targetHealth)
    {
        foreach(Health health in healthOfUnitsAlreadyHitWithProjectile)
        {
            if(health == targetHealth)
            {
                return false;
            }
        }

        return true;
    }
}
