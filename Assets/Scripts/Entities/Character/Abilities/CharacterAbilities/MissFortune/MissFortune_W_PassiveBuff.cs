using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_W_PassiveBuff : AbilityBuff
{
    private float baseBuffFlatBonus;
    private float timeBeforeFullBuffPower;

    protected MissFortune_W_PassiveBuff()
    {
        buffName = "Strut";

        baseBuffFlatBonus = 25;
        timeBeforeFullBuffPower = 5;

        buffFlatBonus = 60;
        buffFlatBonusPerLevel = 10;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_PassiveBuff";
    }

    /*public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.AddFlatBonus(baseBuffFlatBonus);

        base.ApplyBuffToEntityHit(entityHit, currentStacks);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.RemoveFlatBonus(baseBuffFlatBonus);

        base.RemoveBuffFromEntityHit(entityHit, currentStacks);
    }*/
}
