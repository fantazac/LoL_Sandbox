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
            character.CharacterMovementManager.SetMoveTowardsTarget(target, range, false);
            character.CharacterMovementManager.CharacterIsInTargetRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Entity target)
    {
        StartAbilityCast();

        if (!character.CharacterMovementManager.IsMovingTowardsTarget() && !character.EntityBasicAttack.CurrentTarget())
        {
            character.CharacterMovementManager.SetMoveTowardsTarget(target, character.EntityStats.AttackRange.GetTotal(), true, true);
        }

        targetedEntity = target;
        destinationOnCast = target.transform.position;
        RotationOnAbilityCast(target.transform.position);

        StartCorrectCoroutine();
    }

    protected virtual void OnAbilityEffectHit(AbilityEffect abilityEffect, Entity entityHit, bool isACriticalStrike, bool willMiss) { }

    public override bool CanBeCast(Vector3 mousePosition) { return false; }
    public override Vector3 GetDestination() { return Vector3.down; }
    public override void UseAbility(Vector3 destination) { }
}
