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
        //buffFlatValue = 10;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCQ_Debuff";
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        //affectedEntity.EntityStats.MovementSpeed.AddFlatMalus(buffValue);
        affectedEntity.EntityStatusManager.AddCrowdControlEffectOnEntity(CrowdControlEffects.STUN);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        //affectedEntity.EntityStats.MovementSpeed.RemoveFlatMalus(buffValue);
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffectFromEntity(CrowdControlEffects.STUN);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        //return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
        return new Buff(this, affectedEntity, buffFlatValue, buffDuration);
    }
}