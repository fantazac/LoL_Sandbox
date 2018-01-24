public class MagicPenetration : Stat
{
    public override string GetUIText()
    {
        return "MAGIC PENETRATION: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
              ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
