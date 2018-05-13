using System.Collections;
using System;
using UnityEngine;

public abstract class ProjectileDirectionTargeted : Projectile
{
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
