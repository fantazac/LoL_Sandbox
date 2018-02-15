﻿using System;
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

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/SummonerAbilities/Teleport";
    }

    public override void UseAbility(Vector3 destination)
    {
        character.CharacterOrientation.StopRotationTowardsCastPoint();
        character.CharacterOrientation.RotateCharacterInstantly(destination);

        transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();

        StartCooldown(startCooldownOnAbilityCast);
    }
}