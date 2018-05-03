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

        showBuffValueOnUI = true;

        baseBuffFlatBonus = 25;
        timeBeforeFullBuffPower = 5;

        buffFlatValue = 60;
        buffFlatValuePerLevel = 10;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_PassiveBuff";
    }

    public override void UpdateBuffOnAffectedEntities(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue)
    {
        foreach (Entity affectedEntity in EntitiesAffectedByBuff)
        {
            Buff buff = affectedEntity.EntityBuffManager.GetBuff(this);
            if (buff != null)
            {
                if (buff.BuffValue != baseBuffFlatBonus)
                {
                    affectedEntity.EntityStats.MovementSpeed.RemoveFlatBonus(oldFlatValue);
                    affectedEntity.EntityStats.MovementSpeed.AddFlatBonus(newFlatValue);
                    buff.SetBuffValueOnUI(newFlatValue);
                }
            }
        }
    }

    public override void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.AddFlatBonus(buffValue);

        base.ApplyBuffToAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.RemoveFlatBonus(buffValue);

        base.RemoveBuffFromAffectedEntity(affectedEntity, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new BuffUpdatingWithDelay(this, affectedEntity, baseBuffFlatBonus, timeBeforeFullBuffPower, buffFlatValue);
    }
}
