using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    protected List<Team> affectedTeams;
    protected List<Type> affectedUnitTypes;

    public List<Unit> UnitsAlreadyHit { get; }

    public delegate void OnAbilityEffectHitHandler(AbilityEffect abilityEffect, Unit unitHit, bool isACriticalStrike, bool willMiss);
    public event OnAbilityEffectHitHandler OnAbilityEffectHit;

    protected AbilityEffect()
    {
        UnitsAlreadyHit = new List<Unit>();
    }

    protected abstract IEnumerator ActivateAbilityEffect();

    protected virtual bool CanAffectTarget(Unit unitHit)
    {
        if (!unitHit.IsTargetable(affectedUnitTypes, affectedTeams)) return false;
        
        foreach (Unit unit in UnitsAlreadyHit)
        {
            if (unitHit == unit)
            {
                return false;
            }
        }

        return true;

    }

    protected void OnAbilityEffectHitTarget(Unit unitHit, bool isACriticalStrike = false, bool willMiss = false)
    {
        OnAbilityEffectHit?.Invoke(this, unitHit, isACriticalStrike, willMiss);
    }

    protected Unit GetUnitHit(Collider collider)
    {
        return collider.GetComponentInParent<Unit>();
    }
}
