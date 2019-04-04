using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargeted
{
    protected void OnTriggerEnter(Collider collider)
    {
        Unit unitHit = GetUnitHit(collider);

        if (!unitHit || !CanAffectTarget(unitHit)) return;

        unitsAlreadyHit.Add(unitHit);
        OnProjectileHitTarget(unitHit, isACriticalStrike, willMiss);
    }
}
