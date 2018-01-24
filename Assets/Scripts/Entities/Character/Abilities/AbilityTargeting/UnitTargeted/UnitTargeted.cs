using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitTargeted : Ability
{
    public override bool CanBeCast(Entity target, CharacterAbilityManager characterAbilityManager)
    {
        return !characterAbilityManager.IsUsingAbilityPreventingAbilityCasts() && IsAValidTarget(target);
    }

    protected bool IsAValidTarget(Entity target)
    {
        return TargetIsValid.CheckIfTargetIsValid(target, affectedUnitType, character.Team);
    }

    public override void UseAbility(Entity target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            UseAbilityInRange(target);
        }
        else
        {
            character.CharacterMovement.SetMoveTowardsTarget(target, range);
            character.CharacterMovement.CharacterIsInRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Entity target)
    {
        StartAbilityCast();

        character.CharacterMovement.StopAllMovement(this);
        character.CharacterOrientation.RotateCharacterInstantly(target.transform.position);

        if (delayCastTime == null)
        {
            StartCoroutine(AbilityWithoutCastTime());
        }
        else
        {
            StartCoroutine(AbilityWithCastTime());
        }
    }

    protected virtual void OnAreaOfEffectHit(AbilityEffect areaOfEffect, Entity entityHit)
    {
        entityHit.GetComponent<Health>().Reduce(damage);
    }

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager) { return false; }
    public override Vector3 GetDestination() { return Vector3.zero; }
    public override void UseAbility(Vector3 destination) { }
}
