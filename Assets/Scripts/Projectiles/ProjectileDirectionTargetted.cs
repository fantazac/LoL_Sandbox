﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileDirectionTargetted : Projectile
{
    protected override IEnumerator Shoot()
    {
        while (Vector3.Distance(transform.position, initialPosition) < range)
        {
            transform.position += transform.forward * Time.deltaTime * speed;

            yield return null;
        }

        OnProjectileReachedEndOfRange();
    }

    protected override bool CanHitTarget(Health targetHealth)
    {
        foreach (Health health in HealthOfUnitsAlreadyHit)
        {
            if (health == targetHealth)
            {
                return false;
            }
        }

        return true;
    }
}
