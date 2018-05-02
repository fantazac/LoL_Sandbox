using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_P_Buff : AbilityBuff
{
    protected MissFortune_P_Buff()
    {
        buffName = "Love Tap";
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneP_Buff";
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity);
    }
}
