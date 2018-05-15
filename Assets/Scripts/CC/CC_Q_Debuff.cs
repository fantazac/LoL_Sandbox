using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Q_Debuff : AbilityBuff
{
    protected CC_Q_Debuff()
    {
        buffName = "CC BTW";

        isADebuff = true;

        buffDuration = 2;
        buffPercentValue = 99;
        buffCrowdControlEffect = CrowdControlEffects.CHARM;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCQ_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffectOnEntity(buffCrowdControlEffect);
        affectedEntity.EntityStats.MovementSpeed.AddPercentMalus(buffValue);
        affectedEntity.EntityStatusMovementManager.SetupMovementBlock(buffCrowdControlEffect, this, character, transform.position);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffectFromEntity(buffCrowdControlEffect);
        affectedEntity.EntityStats.MovementSpeed.RemovePercentMalus(buffValue);
        affectedEntity.EntityStatusMovementManager.EndMovementBlock(buffCrowdControlEffect, this);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}