using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : AbilityEffect
{
    protected AreaOfEffectCollider[] aoeColliders;

    private WaitForSeconds delayBeforeDestroy;

    public delegate void OnAreaOfEffectHitHandler(AreaOfEffect areaOfEffect, Unit unitHit);
    public event OnAreaOfEffectHitHandler OnAreaOfEffectHit;

    protected void Awake()
    {
        aoeColliders = GetComponentsInChildren<AreaOfEffectCollider>();
    }

    public void CreateAreaOfEffect(List<Team> affectedTeams, List<Type> affectedUnitTypes, float duration)
    {
        this.affectedTeams = affectedTeams;
        this.affectedUnitTypes = affectedUnitTypes;
        delayBeforeDestroy = new WaitForSeconds(duration);
    }

    public void ActivateAreaOfEffect()
    {
        StartCoroutine(ActivateArea());
    }

    protected virtual IEnumerator ActivateArea()
    {
        HitAffectedUnits();

        yield return delayBeforeDestroy;

        Destroy(gameObject);
    }

    protected void HitAffectedUnits()
    {
        foreach (AreaOfEffectCollider aoeCollider in aoeColliders)
        {
            foreach (Collider other in aoeCollider.GetCollidersInAreaOfEffect())
            {
                Unit unitHit = GetUnitHit(other);

                if (!unitHit || !CanAffectTarget(unitHit)) continue;

                unitsAlreadyHit.Add(unitHit);
                OnAreaOfEffectHitTarget(unitHit);
            }
        }
    }

    private void OnAreaOfEffectHitTarget(Unit unitHit)
    {
        OnAreaOfEffectHit?.Invoke(this, unitHit);
    }
}
