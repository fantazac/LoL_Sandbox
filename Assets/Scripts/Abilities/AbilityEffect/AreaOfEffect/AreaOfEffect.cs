using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : AbilityEffect
{
    private AreaOfEffectCollider[] aoeColliders;
    
    private float duration;
    private WaitForSeconds delayBeforeDestroy;

    private void Awake()
    {
        aoeColliders = GetComponentsInChildren<AreaOfEffectCollider>();
    }

    public void CreateAreaOfEffect(List<Team> affectedTeams, List<Type> affectedUnitTypes, float duration)
    {
        this.affectedTeams = affectedTeams;
        this.affectedUnitTypes = affectedUnitTypes;
        this.duration = duration;
        delayBeforeDestroy = new WaitForSeconds(duration);
    }

    public void ActivateAreaOfEffect()
    {
        StartCoroutine(ActivateAbilityEffect());
    }

    protected override IEnumerator ActivateAbilityEffect()
    {
        HitAffectedUnits();

        yield return delayBeforeDestroy;

        Destroy(gameObject);
    }

    private void HitAffectedUnits()
    {
        Vector3 center = transform.position;
        Quaternion rotation = transform.rotation;

        foreach (AreaOfEffectCollider aoeCollider in aoeColliders)
        {
            foreach(Collider other in aoeCollider.GetCollidersInAreaOfEffect())
            {
                Unit unitHit = GetUnitHit(other);

                if (!unitHit || !CanAffectTarget(unitHit)) continue;

                unitsAlreadyHit.Add(unitHit);
                OnAbilityEffectHitTarget(unitHit);
            }
        }
    }
}
