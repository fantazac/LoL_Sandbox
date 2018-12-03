public class Old_Ezreal_P_Buff : AbilityBuff
{
    protected Old_Ezreal_P_Buff()
    {
        buffName = "Rising Spell Force";

        buffDuration = 6;
        buffMaximumStacks = 5;
        buffPercentValue = 10;
        buffPercentValuePerLevel = 2;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP_Buff";
    }

    public override void UpdateBuffOnAffectedUnits(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue)
    {
        foreach (Unit affectedUnit in UnitsAffectedByBuff)
        {
            Buff buff = affectedUnit.BuffManager.GetBuff(this);
            if (buff != null)
            {
                int currentStacks = buff.CurrentStacks;
                affectedUnit.StatsManager.AttackSpeed.RemovePercentBonus(oldPercentValue * currentStacks);
                affectedUnit.StatsManager.AttackSpeed.AddPercentBonus(newPercentValue * currentStacks);
                buff.SetBuffValue(newPercentValue);
            }
        }
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue * buff.CurrentStacks);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue * buff.CurrentStacks);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue, buffDuration, buffMaximumStacks);
    }
}
