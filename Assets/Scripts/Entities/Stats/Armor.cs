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
            return 2 - (100 / (100 - total));
        }
    }

    public float GetPhysicalEffectiveHealthPercent()
    {
        return 1 + (GetPhysicalDamageReductionPercent() / GetPhysicalDamageTakenMultiplier());
    }

    public float GetPhysicalDamageReductionPercent()
    {
        return 1 - GetPhysicalDamageTakenMultiplier();
    }

    protected override string GetSimpleUIText()
    {
        return "ARMOR: " + GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() +
               ") - Takes " + (int)Mathf.Round(GetPhysicalDamageReductionPercent() * 100) + "% reduced physical damage (Eff. HP: " +
               GetPhysicalEffectiveHealthPercent() * 100 + "%)";
    }
}
