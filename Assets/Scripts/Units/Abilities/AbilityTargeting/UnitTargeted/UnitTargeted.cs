﻿using UnityEngine;

public abstract class UnitTargeted : Ability
{
    public override bool CanBeCast(Unit target)
    {
        return IsAValidTarget(target);
    }

    protected bool IsAValidTarget(Unit target)
    {
        return TargetIsValid.CheckIfTargetIsValid(target, affectedUnitType, champion.Team);
    }

    public override void UseAbility(Unit target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            UseAbilityInRange(target);
        }
        else if (!champion.StatusManager.CrowdControlEffectsOnCharacter.Contains(CrowdControlEffect.ROOT))
        {
            champion.MovementManager.SetMoveTowardsTarget(target, range, false);
            champion.MovementManager.CharacterIsInTargetRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Unit target)
    {
        StartAbilityCast();

        if (!champion.MovementManager.IsMovingTowardsTarget() && !champion.BasicAttack.CurrentTarget())
        {
            champion.MovementManager.SetMoveTowardsTarget(target, champion.StatsManager.AttackRange.GetTotal(), true, true);
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
