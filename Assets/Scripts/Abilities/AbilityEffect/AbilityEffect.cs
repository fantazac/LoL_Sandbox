using System;
using System.Collections;
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

    protected abstract IEnumerator ActivateAbilityEffect();

    protected virtual bool CanAffectTarget(Unit unitHit)
    {
        if (!unitHit.IsTargetable(affectedUnitTypes, affectedTeams)) return false;
        
        foreach (Unit unit in unitsAlreadyHit)
        {
            if (unitHit == unit)
            {
                return false;
            }
        }

        return true;

    }

    protected Unit GetUnitHit(Collider other)
    {
        return other.GetComponentInParent<Unit>();
    }
}
