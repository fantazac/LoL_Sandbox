using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_W_Debuff : AbilityBuff
{
    protected CC_W_Debuff()
    {
        buffName = "CC BTW HAHA";

        isADebuff = true;

        buffDuration = 2;
        //buffFlatValue = 10;
        buffCrowdControlEffect = CrowdControlEffects.CHARM;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCW_Debuff";
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        //affectedEntity.EntityStats.MovementSpeed.AddFlatMalus(buffValue);
        affectedEntity.EntityStatusManager.AddCrowdControlEffectOnEntity(buffCrowdControlEffect);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        //affectedEntity.EntityStats.MovementSpeed.RemoveFlatMalus(buffValue);
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffectFromEntity(buffCrowdControlEffect);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        //return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
        return new Buff(this, affectedEntity, buffFlatValue, buffDuration);
    }
}