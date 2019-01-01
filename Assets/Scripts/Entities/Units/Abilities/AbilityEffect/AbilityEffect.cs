using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    protected Team castingUnitTeam;
    protected AbilityAffectedUnitType affectedUnitType;

    public List<Unit> UnitsAlreadyHit { get; protected set; }

    public delegate void OnAbilityEffectHitHandler(AbilityEffect abilityEffect, Unit unitHit, bool isACriticalStrike, bool willMiss);
    public event OnAbilityEffectHitHandler OnAbilityEffectHit;

    protected AbilityEffect()
    {
        UnitsAlreadyHit = new List<Unit>();
    }

    protected abstract IEnumerator ActivateAbilityEffect();

    protected virtual void OnTriggerEnter(Collider collider) { }

    protected virtual bool CanAffectTarget(Unit unitHit)
    {
        if (TargetIsValid.CheckIfTargetIsValid(unitHit, affectedUnitType, castingUnitTeam))
        {
            foreach (Unit unit in UnitsAlreadyHit)
            {
                if (unitHit == unit)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    protected void OnAbilityEffectHitTarget(Unit unitHit, bool isACriticalStrike = false, bool willMiss = false)
    {
        if (OnAbilityEffectHit != null)
        {
            OnAbilityEffectHit(this, unitHit, isACriticalStrike, willMiss);
        }
    }

    protected Unit GetUnitHit(Collider collider)
    {
        return collider.GetComponentInParent<Unit>();
    }
}
