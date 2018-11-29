public class Varus_E_Debuff : AbilityBuff
{
    protected Varus_E_Debuff()
    {
        buffName = "Hail of Arrows";

        isADebuff = true;

        buffPercentValue = 25; // 25/30/35/40/45
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
        affectedEntity.StatsManager.GrievousWounds.AddGrievousWoundsSource();
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.StatsManager.MovementSpeed.RemovePercentMalus(buff.BuffValue);
        affectedEntity.StatsManager.GrievousWounds.RemoveGrievousWoundsSource();
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue);
    }
}
