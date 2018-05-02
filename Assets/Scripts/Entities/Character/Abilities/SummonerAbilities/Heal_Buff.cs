using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Buff : AbilityBuff
{
    private float ratioIfAffectedByHealDebuff;

    private Heal_Debuff healDebuff;

    protected Heal_Buff()
    {
        buffName = "Heal";

        buffDuration = 1;
        buffFlatBonus = 90;
        buffFlatBonusPerLevel = 15;
        buffPercentBonus = 30;

        ratioIfAffectedByHealDebuff = 0.5f;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Ghost";
    }

    protected override void Start()
    {
        base.Start();
        
        healDebuff = GetComponent<Heal_Debuff>();
    }

    public override void ApplyBuffToAffectedEntity(Entity entityHit, float buffValue, int currentStacks)
    {
        if (entityHit.EntityBuffManager.GetDebuffOfSameType(healDebuff) != null)
        {
            entityHit.EntityStats.Health.Restore(buffFlatBonus * ratioIfAffectedByHealDebuff);
        }
        else
        {
            entityHit.EntityStats.Health.Restore(buffFlatBonus);
        }
        entityHit.EntityStats.MovementSpeed.AddPercentBonus(buffPercentBonus);

        base.ApplyBuffToAffectedEntity(entityHit, buffValue, currentStacks);
    }

    public override void RemoveBuffFromAffectedEntity(Entity entityHit, float buffValue, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.RemovePercentBonus(buffPercentBonus);

        base.RemoveBuffFromAffectedEntity(entityHit, buffValue, currentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffFlatBonus, buffDuration);
    }
}
