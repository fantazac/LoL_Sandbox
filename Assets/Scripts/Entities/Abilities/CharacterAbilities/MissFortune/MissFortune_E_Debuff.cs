public class MissFortune_E_Debuff : AbilityBuff
{
    protected MissFortune_E_Debuff()
    {
        buffName = "Make It Rain";

        isADebuff = true;

        buffPercentValue = 28; // 28/36/44/52/60
        buffPercentValuePerLevel = 8;
        buffCrowdControlEffect = CrowdControlEffect.SLOW;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneE_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityStatsManager.MovementSpeed.AddPercentMalus(buffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityStatsManager.MovementSpeed.RemovePercentMalus(buffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue);
    }
}
