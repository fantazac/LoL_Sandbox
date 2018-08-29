public class HealthRegeneration : Stat
{
    protected override string GetSimpleUIText()
    {
        return "HEALTH REGENERATION: " + GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
