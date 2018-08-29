public class AbilityPower : Stat
{
    protected override string GetSimpleUIText()
    {
        return "ABILITY POWER: " + GetTotal();
    }

    protected override string GetUIText()
    {
        // TODO: check if using StringBuilder for all stats would be faster
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
