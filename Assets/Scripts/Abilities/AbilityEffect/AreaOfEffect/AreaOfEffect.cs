using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : AbilityEffect
{
    private float duration;
    private WaitForSeconds delayBeforeDestroy;

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

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 halfExtents = transform.GetChild(i).localScale * 0.5f;
            foreach (Collider other in Physics.OverlapBox(center, halfExtents, rotation))
            {
                Unit unitHit = GetUnitHit(other);

                if (!unitHit || !CanAffectTarget(unitHit)) continue;

                UnitsAlreadyHit.Add(unitHit);
                OnAbilityEffectHitTarget(unitHit);
            }
        }
    }
}
