﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSingleTarget : ProjectileDirectionTargeted
{
    protected bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected override void OnTriggerEnter(Collider collider)
    {
        if (!alreadyHitATarget)
        {
            Entity entityHit = collider.gameObject.GetComponent<Entity>();

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
