using UnityEngine;

public class Armor : Stat
{
    public float GetPhysicalDamageTakenMultiplier()
    {
        if (total >= 0)
        {
            return 100 / (100 + total);
        }
        else
        {
            return 2 - 100 / (100 - total);
        }
    }

    public int GetPhysicalDamageReductionPercent()
    {
        float percent = (1 - GetPhysicalDamageTakenMultiplier())*100;
        return (int)Mathf.Round(percent);
    }

    public override string GetUIText()
    {
        return "ARMOR: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + 
               ") - Takes " + GetPhysicalDamageReductionPercent() + "% reduced physical damage";
    }
}
