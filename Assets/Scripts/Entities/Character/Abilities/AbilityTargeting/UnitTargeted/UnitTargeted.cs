﻿using System.Collections;
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
        else
        {
            character.CharacterMovement.SetMoveTowardsTarget(target, range, false);
            character.CharacterMovement.CharacterIsInRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Entity target)
    {
        StartAbilityCast();

        character.CharacterMovement.StopAllMovement();
        character.CharacterMovement.SetMoveTowardsTarget(target, character.EntityStats.AttackRange.GetTotal(), true);

        destinationOnCast = target.transform.position;
        RotationOnAbilityCast(target.transform.position);

        if (delayCastTime != null)
        {
            StartCoroutine(AbilityWithCastTime());
        }
        else if (delayChannelTime != null)
        {
            StartCoroutine(AbilityWithChannelTime());
        }
        else
        {
            StartCoroutine(AbilityWithoutDelay());
        }
    }

    protected virtual void OnAreaOfEffectHit(AbilityEffect areaOfEffect, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(damage);
        AbilityHit();
    }

    public override bool CanBeCast(Vector3 mousePosition) { return false; }
    public override Vector3 GetDestination() { return Vector3.down; }
    public override void UseAbility(Vector3 destination) { }
}
