public class HealthRegeneration : Stat //the value of this stat is: per 5 seconds of play, you regenerate this much health
{
    public override string GetUIText()
    {
        return "HEALTH REGENERATION: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
