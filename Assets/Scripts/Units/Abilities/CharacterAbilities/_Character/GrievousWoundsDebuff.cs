﻿public class GrievousWoundsDebuff : AbilityBuff
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
