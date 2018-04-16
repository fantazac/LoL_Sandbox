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

    public override void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff buff = entityHit.EntityBuffManager.GetBuff(this);
        if (buff == null)
        {
            buff = new Buff(this, entityHit);
            entityHit.EntityBuffManager.ApplyBuff(buff, buffSprite);
        }
    }
}
