using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargeted
{
    protected override void OnTriggerEnter(Collider collider)
    {
        Entity entityHit = collider.gameObject.GetComponent<Entity>();

        if (entityHit != null && CanAffectTarget(entityHit))
        {
            UnitsAlreadyHit.Add(entityHit);
            OnAbilityEffectHitTarget(entityHit);
        }
    }
}
