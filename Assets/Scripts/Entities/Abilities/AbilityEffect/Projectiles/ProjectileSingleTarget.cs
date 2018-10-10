using UnityEngine;

public class ProjectileSingleTarget : ProjectileDirectionTargeted
{
    protected bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected override void OnTriggerEnter(Collider collider)
    {
        if (!alreadyHitATarget)
        {
            Entity entityHit = GetEntityHit(collider);

            if (entityHit != null && CanAffectTarget(entityHit))
            {
                UnitsAlreadyHit.Add(entityHit);
                OnAbilityEffectHitTarget(entityHit);
                alreadyHitATarget = true;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
