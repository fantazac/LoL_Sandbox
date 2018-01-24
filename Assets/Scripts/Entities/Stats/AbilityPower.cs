public class AbilityPower : Stat
{
    public override string GetUIText()
    {
        // TODO: check if using StringBuilder for all stats would be faster
        return "ABILITY POWER: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
