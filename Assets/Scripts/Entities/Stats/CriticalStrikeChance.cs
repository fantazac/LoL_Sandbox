public class CriticalStrikeChance : Stat
{
    public override string GetUIText()
    {
        return "CRITICAL STRIKE CHANCE: " + GetTotal() + "% ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
