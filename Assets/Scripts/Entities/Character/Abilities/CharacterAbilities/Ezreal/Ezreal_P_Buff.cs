using System;
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

    public override void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff buff = entityHit.EntityBuffManager.GetBuff(this);
        if (buff == null)
        {
            buff = new Buff(this, entityHit, buffDuration, buffMaximumStacks);
            entityHit.EntityBuffManager.ApplyBuff(buff, buffSprite);
        }
        else
        {
            buff.IncreaseCurrentStacks();
            buff.ResetDurationRemaining();
        }
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus * currentStacks);

        base.ApplyBuffToEntityHit(entityHit, currentStacks);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus * currentStacks);

        base.RemoveBuffFromEntityHit(entityHit, currentStacks);
    }
}
