using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recall_Buff : AbilityBuff
{
    protected Recall_Buff()
    {
        buffName = "Recall";
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/_Character/Recall";
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity);
    }
}
