using UnityEngine;

public class Tristana_R_Debuff : AbilityBuff
{
    private float knockbackSpeed;

    protected Tristana_R_Debuff()
    {
        buffName = "Buster Shot";

        isADebuff = true;

        buffFlatValue = 6;// 600/800/1000 * StaticObjects.MultiplyingFactor
        buffFlatValuePerLevel = 2;

        buffCrowdControlEffect = CrowdControlEffects.KNOCKBACK;

        knockbackSpeed = 20;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaR_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffectOnEntity(buffCrowdControlEffect);
        affectedEntity.EntityStatusMovementManager.SetupMovementBlock(buffCrowdControlEffect, this, character, normalizedVector * buffFlatValue, 0, knockbackSpeed);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffectFromEntity(buffCrowdControlEffect);
        affectedEntity.EntityStatusMovementManager.EndMovementBlock(buffCrowdControlEffect, this);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}