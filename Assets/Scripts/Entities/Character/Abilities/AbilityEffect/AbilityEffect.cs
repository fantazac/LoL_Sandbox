using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    protected EntityTeam teamOfCallingEntity;
    protected AbilityAffectedUnitType affectedUnitType;

    public List<Entity> UnitsAlreadyHit { get; protected set; }

    public delegate void OnAbilityEffectHitHandler(AbilityEffect abilityEffect, Entity entityHit);
    public event OnAbilityEffectHitHandler OnAbilityEffectHit;

    protected AbilityEffect()
    {
        UnitsAlreadyHit = new List<Entity>();
    }

    protected abstract IEnumerator ActivateAbilityEffect();

    protected virtual void OnTriggerEnter(Collider collider) { }

    protected virtual bool CanAffectTarget(Entity entityHit)
    {
        if (TargetIsValid.CheckIfTargetIsValid(entityHit, affectedUnitType, teamOfCallingEntity))
        {
            foreach (Entity entity in UnitsAlreadyHit)
            {
                if (entityHit == entity)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    protected void OnAbilityEffectHitTarget(Entity entityHit)
    {
        if (OnAbilityEffectHit != null)
        {
            OnAbilityEffectHit(this, entityHit);
        }
    }
}
