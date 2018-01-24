//TODO: add Lethality and armor reduction
public class ArmorPenetration : Stat
{
    public override string GetUIText()
    {
        return "ARMOR PENETRATION: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
