using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : GroundTargetedBlink//TODO: UnitTargeted
{
    protected Teleport()
    {
        abilityName = "Teleport";

        abilityType = AbilityType.Blink;

        baseCooldown = 300;
        cooldownBeforeRecast = 0.75f;
        channelTime = 4.5f;
        delayChannelTime = new WaitForSeconds(channelTime);

        baseCooldownOnCancel = 200;

        CanBeRecasted = true;
        CannotCastAnyAbilityWhileActive = true;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Teleport";
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovement.RestartMovementTowardsTargetAfterAbility();

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }
}
