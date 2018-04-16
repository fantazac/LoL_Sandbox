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

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/_Character/Recall";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Recall_Buff>() };

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromEntityHit;
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

        AbilityBuffs[0].ConsumeBuff(character);

        transform.position = GetDestination();
        character.CharacterMovement.NotifyCharacterMoved();

        IsBeingChanneled = false;
        FinishAbilityCast();
    }

    private void AddNewBuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced += CancelRecall;
        ((Character)entityHit).CharacterAbilityManager.OnAnAbilityUsed += CancelRecall;
        ((Character)entityHit).CharacterMovement.CharacterMoved += CancelRecall;
        //TODO: if cc'd, cancel aswell
        AbilityBuffs[0].AddNewBuffToEntityHit(entityHit);
    }

    private void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= CancelRecall;
        ((Character)entityHit).CharacterAbilityManager.OnAnAbilityUsed -= CancelRecall;
        ((Character)entityHit).CharacterMovement.CharacterMoved -= CancelRecall;
        //TODO: remove cc cancel
    }

    private void CancelRecall()
    {
        IsBeingChanneled = false;
        CancelAbility();
        AbilityBuffs[0].ConsumeBuff(character);
    }
}
