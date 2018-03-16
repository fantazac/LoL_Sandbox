using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recall : AutoTargetedBlink, OtherAbility
{
    protected Recall()
    {
        abilityName = "Recall";

        abilityType = AbilityType.Blink;

        castTime = 0.5f;
        channelTime = 8;
        delayCastTime = new WaitForSeconds(castTime);
        delayChannelTime = new WaitForSeconds(channelTime);
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
        yield return delayCastTime;

        AddNewBuffToEntityHit(character);

        yield return delayChannelTime;

        ConsumeBuff(character);

        transform.position = GetDestination();
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        EntitiesAffectedByBuff.Remove(entityHit);
    }
}
