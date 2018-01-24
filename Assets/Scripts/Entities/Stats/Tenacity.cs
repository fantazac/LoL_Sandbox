public class Tenacity : Stat
{
    public override string GetUIText()
    {
        return "TENACITY: " + GetTotal() + "% ((" + GetBaseValue() + " + " + GetFlatBonus() + ") * " +
            GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
