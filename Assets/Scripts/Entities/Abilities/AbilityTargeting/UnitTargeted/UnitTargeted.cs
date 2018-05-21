using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitTargeted : Ability
{
    public override bool CanBeCast(Entity target)
    {
        return IsAValidTarget(target);
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
        else if (!character.EntityStatusManager.CrowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ROOT))
        {
            character.CharacterMovement.SetMoveTowardsTarget(target, range, false);
            character.CharacterMovement.CharacterIsInTargetRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Entity target)
    {
        StartAbilityCast();
        
        if (!character.CharacterMovement.IsMovingTowardsTarget() && !character.EntityBasicAttack.CurrentTarget())
        {
            character.CharacterMovement.SetMoveTowardsTarget(target, character.EntityStats.AttackRange.GetTotal(), true, true);
        }

        targetedEntity = target;
        destinationOnCast = target.transform.position;
        RotationOnAbilityCast(target.transform.position);

        StartCorrectCoroutine();
    }

    protected virtual void OnAbilityEffectHit(AbilityEffect abilityEffect, Entity entityHit) { }

    public override bool CanBeCast(Vector3 mousePosition) { return false; }
    public override Vector3 GetDestination() { return Vector3.down; }
    public override void UseAbility(Vector3 destination) { }
}
