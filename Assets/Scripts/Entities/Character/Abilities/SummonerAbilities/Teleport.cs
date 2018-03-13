using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : GroundTargetedBlink, SummonerAbility
{
    protected Teleport()
    {
        abilityName = "Teleport";

        cooldown = 300;
        cooldownBeforeRecast = 0.75f;
        channelTime = 4.5f;
        delayChannelTime = new WaitForSeconds(channelTime);

        CanBeRecasted = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Teleport";
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovement.StopMovementTowardsPoint();

        FinalAdjustments(destination);

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
}
