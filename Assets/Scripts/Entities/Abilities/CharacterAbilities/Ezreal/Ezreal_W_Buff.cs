using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W_Buff : AbilityBuff
{
    protected Ezreal_W_Buff()
    {
        buffName = "Essence Flux";

        buffDuration = 5;
        buffPercentValue = 20;
        buffPercentValuePerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW_Buff";
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.AttackSpeed.AddPercentBonus(buffValue);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.AttackSpeed.RemovePercentBonus(buffValue);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration, buffMaximumStacks);
    }
}
