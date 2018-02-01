using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : GroundTargetedBlink, SummonerAbility
{
    protected Teleport()
    {
        cooldown = 13;

        startCooldownOnAbilityCast = true;
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        this.destination = destination;
    }

    public override void UseAbility(Vector3 destination)//TODO FIX ME: The crash bug if you do FinishAbilityCast in this method instead of a coroutine.
    {
        StartAbilityCast();

        character.CharacterOrientation.RotateCharacterInstantly(destination);

        FinalAdjustments(destination);

        StartCoroutine(AbilityWithCastTime());
    }
}
