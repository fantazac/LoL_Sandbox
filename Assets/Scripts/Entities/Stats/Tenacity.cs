public class Tenacity : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    protected override string GetSimpleUIText()
    {
        return "TENACITY: " + GetTotal() + "%";
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
