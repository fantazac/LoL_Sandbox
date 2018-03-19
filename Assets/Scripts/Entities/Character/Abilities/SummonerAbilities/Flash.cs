using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : GroundTargetedBlink, SummonerAbility
{
    protected Flash()
    {
        abilityName = "Flash";

        abilityType = AbilityType.Blink;

        range = 400;
        cooldown = 300;

        startCooldownOnAbilityCast = true;

        CanBeCastDuringOtherAbilityCastTimes = true;

        IsEnabled = true;
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
