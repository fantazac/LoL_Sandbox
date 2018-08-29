public class ArmorPenetrationPercent : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    protected override string GetSimpleUIText()
    {
        return "ARMOR PENETRATION PERCENT: " + GetTotal() + "%";
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
