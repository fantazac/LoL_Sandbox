using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recall : AutoTargetedBlink, OtherCharacterAbility
{
    protected Recall()
    {
        abilityName = "Recall";

        abilityType = AbilityType.Blink;

        castTime = 0.5f;
        channelTime = 8;
        delayCastTime = new WaitForSeconds(castTime);
        delayChannelTime = new WaitForSeconds(channelTime);

        CanMoveWhileChanneling = true;
        CanUseAnyAbilityWhileChanneling = true;

        IsEnabled = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/_Character/Recall";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/_Character/Recall";
    }

    public override Vector3 GetDestination()
    {
        return character.CharacterMovement.CharacterHeightOffset;
    }

    protected override IEnumerator AbilityWithCastTimeAndChannelTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterMovement.StopAllMovement();
        AddNewBuffToEntityHit(character);
        IsBeingChanneled = true;

        yield return delayChannelTime;

        ConsumeBuff(character);

        transform.position = GetDestination();
        character.CharacterMovement.NotifyCharacterMoved();

        IsBeingChanneled = false;
        FinishAbilityCast();
    }

    private void CancelRecall()
    {
        IsBeingChanneled = false;
        CancelAbility();
        ConsumeBuff(character);
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.Health.OnHealthReduced += CancelRecall;
        ((Character)entityHit).CharacterAbilityManager.OnAnAbilityUsed += CancelRecall;
        ((Character)entityHit).CharacterMovement.CharacterMoved += CancelRecall;
        //TODO: if cc'd, cancel aswell
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= CancelRecall;
        ((Character)entityHit).CharacterAbilityManager.OnAnAbilityUsed -= CancelRecall;
        ((Character)entityHit).CharacterMovement.CharacterMoved -= CancelRecall;
        //TODO: remove cc cancel
        EntitiesAffectedByBuff.Remove(entityHit);
    }
}
