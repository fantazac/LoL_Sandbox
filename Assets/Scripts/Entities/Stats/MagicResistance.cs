using UnityEngine;

public class MagicResistance : Stat
{
    public float GetMagicDamageTakenMultiplier()
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

    public float GetMagicEffectiveHealthPercent()
    {
        return 1 + (GetMagicDamageReductionPercent() / GetMagicDamageTakenMultiplier());
    }

    public float GetMagicDamageReductionPercent()
    {
        return 1 - GetMagicDamageTakenMultiplier();
    }

    protected override string GetSimpleUIText()
    {
        return "MAGIC RESISTANCE: " + GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() +
               ") - Takes " + (int)Mathf.Round(GetMagicDamageReductionPercent() * 100) + "% reduced magic damage (Eff. HP: " + 
               GetMagicEffectiveHealthPercent() * 100 + "%)";
    }
}
