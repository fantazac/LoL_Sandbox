public class LifeSteal : Stat
{
    public override string GetUIText()
    {
        return "LIFE STEAL: " + GetTotal() + "% ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
