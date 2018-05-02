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

    public override void AddNewBuffToAffectedEntity(Entity affectedEntity)
    {
        SetupBuff(affectedEntity.EntityBuffManager.GetDebuffOfSameType(this), affectedEntity, true);
    }

    protected override void SetupBuff(Buff buff, Entity affectedEntity, bool isADebuff = false)
    {
        if (buff != null)
        {
            Consume(buff);
        }
        affectedEntity.EntityBuffManager.ApplyBuff(CreateNewBuff(affectedEntity), buffSprite, isADebuff);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, buffDuration);
    }
}
