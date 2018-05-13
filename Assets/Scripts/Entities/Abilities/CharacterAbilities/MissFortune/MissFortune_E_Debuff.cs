using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_E_Debuff : AbilityBuff
{
    protected MissFortune_E_Debuff()
    {
        buffName = "Make It Rain";

        isADebuff = true;

        buffPercentValue = 28; // 28/36/44/52/60
        buffPercentValuePerLevel = 8;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneE_Debuff";
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.AddPercentMalus(buffValue);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.RemovePercentMalus(buffValue);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue);
    }
}
