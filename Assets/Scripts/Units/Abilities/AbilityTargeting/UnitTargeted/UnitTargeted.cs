using UnityEngine;

public abstract class UnitTargeted : Ability
{
    public override bool CanBeCast(Unit target)
    {
        return IsAValidTarget(target);
    }

    protected bool IsAValidTarget(Unit target)
    {
        return TargetIsValid.CheckIfTargetIsValid(target, affectedUnitType, character.Team);
    }

    public override void UseAbility(Unit target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            UseAbilityInRange(target);
        }
        else if (!character.StatusManager.CrowdControlEffectsOnCharacter.Contains(CrowdControlEffect.ROOT))
        {
            character.CharacterMovementManager.SetMoveTowardsTarget(target, range, false);
            character.CharacterMovementManager.CharacterIsInTargetRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Unit target)
    {
        StartAbilityCast();

        if (!character.CharacterMovementManager.IsMovingTowardsTarget() && !character.BasicAttack.CurrentTarget())
        {
            character.CharacterMovementManager.SetMoveTowardsTarget(target, character.StatsManager.AttackRange.GetTotal(), true, true);
        }

        targetedUnit = target;
        destinationOnCast = target.transform.position;
        RotationOnAbilityCast(target.transform.position);

        StartCorrectCoroutine();
    }

    protected virtual void OnAbilityEffectHit(AbilityEffect abilityEffect, Unit unitHit, bool isACriticalStrike, bool willMiss) { }

    public override bool CanBeCast(Vector3 mousePosition) { return false; }
    public override Vector3 GetDestination() { return Vector3.down; }
    public override void UseAbility(Vector3 destination) { }
}
