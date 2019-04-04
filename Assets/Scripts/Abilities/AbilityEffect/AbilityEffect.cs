using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    protected List<Team> affectedTeams;
    protected List<Type> affectedUnitTypes;

    protected readonly List<Unit> unitsAlreadyHit;

    protected AbilityEffect()
    {
        unitsAlreadyHit = new List<Unit>();
    }

    protected virtual bool CanAffectTarget(Unit unitHit)
    {
        return !unitsAlreadyHit.Contains(unitHit) && unitHit.IsTargetable(affectedUnitTypes, affectedTeams);
    }

    protected Unit GetUnitHit(Collider other)
    {
        return other.GetComponentInParent<Unit>();
    }
}
