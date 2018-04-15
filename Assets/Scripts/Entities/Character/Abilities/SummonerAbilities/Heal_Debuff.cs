using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Debuff : AbilityBuff
{
    protected Heal_Debuff()
    {
        buffName = "Heal";

        isADebuff = true;

        buffDuration = 35;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    public override void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff debuff = entityHit.EntityBuffManager.GetDebuffOfSameType(this);
        if (debuff != null)
        {
            debuff.ConsumeBuff();
        }
        debuff = new Buff(this, entityHit, buffDuration);
        entityHit.EntityBuffManager.ApplyDebuff(debuff, buffSprite);
    }
}
