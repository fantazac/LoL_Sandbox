public class SpellVamp : Stat
{
    public override string GetUIText()
    {
        return "SPELL VAMP: " + GetTotal() + "% ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
