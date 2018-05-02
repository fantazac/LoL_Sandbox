﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_P_Buff : AbilityBuff
{
    protected Ezreal_P_Buff()
    {
        buffName = "Rising Spell Force";

        buffDuration = 6;
        buffMaximumStacks = 5;
        buffPercentBonus = 10;
        buffPercentBonusPerLevel = 2;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP_Buff";
    }

    public override void UpdateBuffOnAffectedEntities(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue)
    {
        foreach (Entity affectedEntity in EntitiesAffectedByBuff)
        {
            Buff buff = affectedEntity.EntityBuffManager.GetBuff(this);
            if (buff != null)
            {
                int currentStacks = buff.CurrentStacks;
                affectedEntity.EntityStats.AttackSpeed.RemovePercentBonus(oldPercentValue * currentStacks);
                affectedEntity.EntityStats.AttackSpeed.AddPercentBonus(newPercentValue * currentStacks);
            }
        }
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus * currentStacks);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus * currentStacks);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentBonus, buffDuration, buffMaximumStacks);
    }
}
