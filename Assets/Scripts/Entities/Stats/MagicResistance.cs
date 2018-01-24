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

    public int GetMagicDamageReductionPercent()
    {
        float percent = (1 - GetMagicDamageTakenMultiplier()) * 100;
        return (int)Mathf.Round(percent);
    }

    public override string GetUIText()
    {
        return "MAGIC RESISTANCE: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() +
            ") - Takes " + GetMagicDamageReductionPercent() + "% reduced magic damage";
    }
}
