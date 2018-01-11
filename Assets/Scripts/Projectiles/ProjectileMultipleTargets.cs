using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargetted
{
    protected override void OnTriggerEnter(Collider collider)
    {
        if (!destroyProjectile)
        {
            Health targetHealth = collider.gameObject.GetComponent<Health>();

            if (targetHealth != null && CanHitTarget(targetHealth))
            {
                healthOfUnitsAlreadyHitWithProjectile.Add(targetHealth);
                targetHealth.Hit(damage);
            }
        }
    }
}
