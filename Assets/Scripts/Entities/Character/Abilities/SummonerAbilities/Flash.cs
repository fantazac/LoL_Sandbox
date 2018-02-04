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
        //StartAbilityCast();

        character.CharacterMovement.StopMovementTowardsPoint();

        Vector3 newDestination = FindPointToMoveTo(destination, transform.position);
        Quaternion newRotation = Quaternion.LookRotation((destination - transform.position).normalized); ;
        transform.position = newDestination;
        character.CharacterAbilityManager.StopAllDashAbilities();
        
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
                transform.rotation = newRotation;
            }
        }
        else if (character.CharacterAbilityManager.IsUsingOnlyDashAbilitiesOrFlash())
        {
            transform.rotation = newRotation;
        }

        character.CharacterMovement.NotifyCharacterMoved();

        //FinishAbilityCast();
    }
}
