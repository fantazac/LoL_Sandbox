using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargeted
{
    protected void OnTriggerEnter(Collider collider)
    {
        Unit unitHit = GetUnitHit(collider);

        if (unitHit != null && CanAffectTarget(unitHit))
        {
            unitsAlreadyHit.Add(unitHit);
            OnAbilityEffectHitTarget(unitHit, isACriticalStrike, willMiss);
        }
    }
}
