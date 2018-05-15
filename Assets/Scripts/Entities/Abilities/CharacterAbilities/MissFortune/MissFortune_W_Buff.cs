using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_W_Buff : AbilityBuff
{
    protected MissFortune_W_Buff()
    {
        buffName = "Guns Blazing";

        buffDuration = 4;
        buffPercentValue = 40;
        buffPercentValuePerLevel = 15;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_Buff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.AttackSpeed.AddPercentBonus(buffPercentValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}
