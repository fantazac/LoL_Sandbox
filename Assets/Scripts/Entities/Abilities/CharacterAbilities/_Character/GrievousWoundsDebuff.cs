public class GrievousWoundsDebuff : AbilityBuff
{
    protected GrievousWoundsDebuff()
    {
        buffName = "Grievous Wounds";

        isADebuff = true;

        buffPercentValue = 40; // 25/30/35/40/45
        buffPercentValuePerLevel = 5;
        buffCrowdControlEffect = CrowdControlEffect.SLOW;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusE_Debuff";
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
