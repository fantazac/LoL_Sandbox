using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_P_Debuff : AbilityBuff
{
    protected MissFortune_P_Debuff()
    {
        buffName = "Love Tap";

        isADebuff = true;
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
