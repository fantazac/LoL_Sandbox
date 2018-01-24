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

    public override string GetUIText()
    {
        return "MAGIC RESISTANCE: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() +
            ") - Takes " + (int)(GetMagicDamageTakenMultiplier()*100) + "% reduced magic damage";
    }
}
