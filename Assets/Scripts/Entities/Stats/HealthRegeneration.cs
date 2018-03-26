public class HealthRegeneration : Stat
{
    public override string GetUIText()
    {
        return "HEALTH REGENERATION: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
