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
        CanRotateWhileCasting = true;
    }

    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/SummonerAbilities/Flash";
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovement.StopMovementTowardsPoint();

        Vector3 newDestination = FindPointToMoveTo(destination, transform.position);
        transform.position = newDestination;
        character.CharacterAbilityManager.StopAllDashAbilities(newDestination);

        if (character.CharacterAbilityManager.IsUsingAbilityThatHasACastTime())
        {
            character.CharacterOrientation.RotateCharacterInstantlyToLastInstantRotation();
        }
        else if (!character.CharacterAbilityManager.IsUsingAbilityPreventingRotation())
        {
            Entity target = character.CharacterMovement.GetTarget();
            if (target != null)
            {
                character.CharacterOrientation.RotateCharacterInstantly(target.transform.position);
            }
            else
            {
                character.CharacterOrientation.RotateCharacterInstantly(destination);
            }
        }
        else if (character.CharacterAbilityManager.IsUsingADashAbility())
        {
            character.CharacterOrientation.RotateCharacterInstantly(destination);
        }

        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }
}
