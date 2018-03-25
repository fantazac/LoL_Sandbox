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

    public override string GetUIText()
    {
        return "ARMOR: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() +
               ") - Takes " + (int)Mathf.Round(GetPhysicalDamageReductionPercent() * 100) + "% reduced physical damage (Eff. HP: " +
               GetPhysicalEffectiveHealthPercent() * 100 + "%)";
    }

    /*
        100/(100+25) = 0.8 (incoming damage)
        1 - 0.8 = 0.2 (damage reduction)
        1 + (0.2 / 0.8) = 1.25 (effective health)

        100/(100+100) = 0.5 (incoming damage)
        1 - 0.5 = 0.5 (damage reduction)
        1 + (0.5 / 0.5) = 2 (effective health)

        2 - (100/(100--25)) = 1.2 (incoming damage)
        1 - 1.2 = -0.2 (damage reduction)
        1 + (-0.2 / 1.2) = 0.8333 (effective health)

        2 - (100/(100--100)) = 1.5 (incoming damage)
        1 - 1.5 = -0.5 (damage reduction)
        1 + (-0.5 / 1.5) = 0.6667 (effective health) 
    */
}
