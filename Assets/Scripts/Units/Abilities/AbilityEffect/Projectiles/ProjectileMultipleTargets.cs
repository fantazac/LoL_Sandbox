using UnityEngine;

public class ProjectileMultipleTargets : ProjectileDirectionTargeted
{
    protected override void OnTriggerEnter(Collider collider)
    {
        Unit unitHit = GetUnitHit(collider);

        if (unitHit != null && CanAffectTarget(unitHit))
        {
            UnitsAlreadyHit.Add(unitHit);
            OnAbilityEffectHitTarget(unitHit, isACriticalStrike, willMiss);
        }
    }
}
