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
        buffPercentBonus = 20;
        buffPercentBonusPerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW_Buff";
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus);
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus);
        EntitiesAffectedByBuff.Remove(entityHit);
    }
}
