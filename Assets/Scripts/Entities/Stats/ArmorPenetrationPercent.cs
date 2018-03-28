public class ArmorPenetrationPercent : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    public override string GetUIText()
    {
        return "ARMOR PENETRATION PERCENT: " + GetTotal() + "% (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
