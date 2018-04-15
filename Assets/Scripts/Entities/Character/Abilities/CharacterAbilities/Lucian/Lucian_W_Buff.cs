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
        buffFlatBonus = 60;// 60/65/70/75/80
        buffFlatBonusPerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW_Buff";
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.AddFlatBonus(buffFlatBonus);

        base.ApplyBuffToEntityHit(entityHit, currentStacks);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.RemoveFlatBonus(buffFlatBonus);

        base.RemoveBuffFromEntityHit(entityHit, currentStacks);
    }
}
