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
        affectedEntity.EntityStatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityStatsManager.MovementSpeed.AddPercentMalus(buff.BuffValue);
        affectedEntity.EntityStatsManager.GrievousWounds.AddGrievousWoundsSource();
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityStatsManager.MovementSpeed.RemovePercentMalus(buff.BuffValue);
        affectedEntity.EntityStatsManager.GrievousWounds.RemoveGrievousWoundsSource();
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue);
    }
}
