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

        knockbackSpeed = 20;

        buffCrowdControlEffect = CrowdControlEffect.KNOCKBACK;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaR_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityDisplacementManager.SetupDisplacement(normalizedVector * buffFlatValue, knockbackSpeed, this);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity);
    }
}