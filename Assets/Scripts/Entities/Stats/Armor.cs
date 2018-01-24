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

    public override string GetUIText()
    {
        return "ARMOR: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + 
               ") - Takes " + (int)(GetPhysicalDamageTakenMultiplier()*100) + "% reduced physical damage";
    }
}
