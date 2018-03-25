public class ArmorPenetrationPercent : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f));
    }

    public override string GetUIText()
    {
        return "ARMOR PENETRATION PERCENT: " + GetTotal() + " ((" + GetBaseValue() + " * " + GetPercentBonus() + ") * " + GetFlatMalus() + ")";
    }
}
