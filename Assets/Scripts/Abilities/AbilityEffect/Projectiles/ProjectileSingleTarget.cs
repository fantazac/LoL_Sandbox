using UnityEngine;

public class ProjectileSingleTarget : ProjectileDirectionTargeted
{
    private bool alreadyHitATarget; //This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected void OnTriggerEnter(Collider collider)
    {
        if (alreadyHitATarget) return;

        Unit unitHit = GetUnitHit(collider);

        if (!unitHit || !CanAffectTarget(unitHit)) return;

        unitsAlreadyHit.Add(unitHit);
        OnProjectileHitTarget(unitHit, isACriticalStrike, willMiss);
        alreadyHitATarget = true;
        GetComponent<Collider>().enabled = false;
    }
}
