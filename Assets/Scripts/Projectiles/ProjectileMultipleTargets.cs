﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargetted
{
    protected override void OnTriggerEnter(Collider collider)
    {
        if (!alreadyHitTarget && (collider.gameObject.GetComponent<Character>().team != teamOfShooter || canHitAllies))
        {
            Health targetHealth = collider.gameObject.GetComponent<Health>();

            if (targetHealth != null && CanHitTarget(targetHealth))
            {
                HealthOfUnitsAlreadyHit.Add(targetHealth);
                targetHealth.Hit(damage);
                OnProjectileHitTarget();
            }
        }
    }
}
