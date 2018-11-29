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

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.StatsManager.MovementSpeed.AddPercentMalus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.StatsManager.MovementSpeed.RemovePercentMalus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue);
    }
}
