using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectWithEffectOverTime : AreaOfEffect
{
    private int numberOfTicks;
    private WaitForSeconds delayPerTick;

    public delegate void OnAreaOfEffectWithEffectOverTimeHitHandler(AreaOfEffect areaOfEffect, List<Unit> previouslyAffectedUnits, List<Unit> affectedUnits);
    public event OnAreaOfEffectWithEffectOverTimeHitHandler OnAreaOfEffectWithEffectOverTimeHit;

    public void CreateAreaOfEffect(List<Team> affectedTeams, List<Type> affectedUnitTypes, WaitForSeconds delayPerTick, int numberOfTicks)
    {
        this.affectedTeams = affectedTeams;
        this.affectedUnitTypes = affectedUnitTypes;
        this.delayPerTick = delayPerTick;
        this.numberOfTicks = numberOfTicks;
    }

    protected override IEnumerator ActivateArea()
    {
        HitAffectedUnits();

        List<Unit> previouslyAffectedUnits = new List<Unit>();

        for (int i = 0; i < numberOfTicks; i++)
        {
            yield return delayPerTick;

            List<Unit> affectedUnits = GetAffectedUnits();
            OnAreaOfEffectWithEffectOverTimeHit?.Invoke(this, previouslyAffectedUnits, affectedUnits);

            previouslyAffectedUnits = affectedUnits;
        }

        yield return delayPerTick;

        OnAreaOfEffectWithEffectOverTimeHit?.Invoke(this, previouslyAffectedUnits, new List<Unit>());

        Destroy(gameObject);
    }

    private List<Unit> GetAffectedUnits()
    {
        List<Unit> affectedUnits = new List<Unit>();

        foreach (AreaOfEffectCollider aoeCollider in aoeColliders)
        {
            foreach (Collider other in aoeCollider.GetCollidersInAreaOfEffect())
            {
                Unit unitHit = GetUnitHit(other);

                if (!unitHit || !CanAffectTarget(unitHit)) continue;

                affectedUnits.Add(unitHit);
            }
        }

        return affectedUnits;
    }

    protected override bool CanAffectTarget(Unit unitHit) // TODO: Check if this method is called when HitAffectedUnits checks for targets (it should call base one)
    {
        return unitHit.IsTargetable(affectedUnitTypes, affectedTeams);
    }
}
