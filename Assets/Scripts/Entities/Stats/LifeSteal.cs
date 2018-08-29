public class LifeSteal : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    protected override string GetSimpleUIText()
    {
        return "LIFE STEAL: " + GetTotal() + "%";
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
