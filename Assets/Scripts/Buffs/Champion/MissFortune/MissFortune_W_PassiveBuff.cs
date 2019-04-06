public class MissFortune_W_PassiveBuff : AbilityBuff
{
    private readonly float baseBuffFlatBonus;

    protected MissFortune_W_PassiveBuff()
    {
        buffName = "Strut";

        showBuffValueOnUI = true;

        baseBuffFlatBonus = 25;
        buffDuration = 5;

        buffFlatValue = 60;
        buffFlatValuePerLevel = 10;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_PassiveBuff";
    }

    protected override void UpdateBuffOnAffectedUnits(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue)
    {
        foreach (Unit affectedUnit in unitsAffectedByBuff)
        {
            BuffUpdatingWithDelay buff = (BuffUpdatingWithDelay)affectedUnit.BuffManager.GetBuff(this);
            
            if (buff == null) continue;
            
            if (buff.BuffValue != baseBuffFlatBonus)
            {
                affectedUnit.StatsManager.MovementSpeed.RemoveFlatBonus(oldFlatValue);
                affectedUnit.StatsManager.MovementSpeed.AddFlatBonus(newFlatValue);
                buff.SetBuffValueOnUI(newFlatValue);
            }
            else
            {
                buff.SetBuffValuePostDelay(newFlatValue);
            }
        }
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.MovementSpeed.AddFlatBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.MovementSpeed.RemoveFlatBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new BuffUpdatingWithDelay(this, affectedUnit, baseBuffFlatBonus, buffDuration, buffFlatValue);
    }
}
