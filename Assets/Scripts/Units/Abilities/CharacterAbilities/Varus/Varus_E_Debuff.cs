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

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedUnit.StatsManager.MovementSpeed.AddPercentMalus(buff.BuffValue);
        affectedUnit.StatsManager.GrievousWounds.AddGrievousWoundsSource();
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedUnit.StatsManager.MovementSpeed.RemovePercentMalus(buff.BuffValue);
        affectedUnit.StatsManager.GrievousWounds.RemoveGrievousWoundsSource();
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue);
    }
}
