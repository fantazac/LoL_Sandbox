using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : GroundTargetedBlink, SummonerAbility
{
    protected Teleport()
    {
        abilityName = "Teleport";

        abilityType = AbilityType.Blink;

        cooldown = 300;
        cooldownBeforeRecast = 0.75f;
        channelTime = 4.5f;
        delayChannelTime = new WaitForSeconds(channelTime);

        CanBeRecasted = true;
        CannotCastAnyAbilityWhileActive = true;
    }

    protected override void SetSpritePaths()
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
