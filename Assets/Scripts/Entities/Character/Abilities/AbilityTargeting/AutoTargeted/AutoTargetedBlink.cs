﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoTargetedBlink : AutoTargeted
{
    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }
}
