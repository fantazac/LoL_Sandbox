using UnityEngine;

public class ProjectileSingleTarget : ProjectileDirectionTargeted
{
    protected bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected void OnTriggerEnter(Collider collider)
    {
        if (!alreadyHitATarget)
        {
            Unit unitHit = GetUnitHit(collider);

            if (unitHit != null && CanAffectTarget(unitHit))
            {
                unitsAlreadyHit.Add(unitHit);
                OnAbilityEffectHitTarget(unitHit, isACriticalStrike, willMiss);
                alreadyHitATarget = true;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
