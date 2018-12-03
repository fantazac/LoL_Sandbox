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

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedUnit.StatsManager.MovementSpeed.AddPercentMalus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedUnit.StatsManager.MovementSpeed.RemovePercentMalus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue);
    }
}
