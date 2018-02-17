using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : GroundTargetedBlink, SummonerAbility
{
    protected Flash()
    {
        range = 400;
        cooldown = 20;

        startCooldownOnAbilityCast = true;

        CanBeCastAtAnytime = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Flash";
    }

    public override void UseAbility(Vector3 destination)
    {
        character.CharacterMovement.StopMovementTowardsPoint();

        Vector3 newDestination = FindPointToMoveTo(destination, transform.position);
        Quaternion newRotation = Quaternion.LookRotation((destination - transform.position).normalized);

        transform.position = newDestination;
        character.CharacterAbilityManager.StopAllDashAbilities();

        if (!character.CharacterAbilityManager.IsUsingAbilityPreventingRotation())
        {
            transform.rotation = newRotation;
        }

        character.CharacterMovement.NotifyCharacterMoved();

        StartCooldown(startCooldownOnAbilityCast);
    }
}
