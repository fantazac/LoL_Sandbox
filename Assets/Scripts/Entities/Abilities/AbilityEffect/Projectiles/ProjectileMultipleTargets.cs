using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargeted
{
    protected override void OnTriggerEnter(Collider collider)
    {
        Entity entityHit = GetEntityHit(collider);

        if (entityHit != null && CanAffectTarget(entityHit))
        {
            UnitsAlreadyHit.Add(entityHit);
            OnAbilityEffectHitTarget(entityHit);
        }
    }
}
