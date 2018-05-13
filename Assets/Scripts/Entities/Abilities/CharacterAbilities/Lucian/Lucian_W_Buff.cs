using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W_Buff : AbilityBuff
{
    protected Lucian_W_Buff()
    {
        buffName = "Ardent Blaze";

        buffDuration = 1;
        buffFlatValue = 60;// 60/65/70/75/80
        buffFlatValuePerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW_Buff";
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.AddFlatBonus(buffFlatValue);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.RemoveFlatBonus(buffFlatValue);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffFlatValue, buffDuration);
    }
}
