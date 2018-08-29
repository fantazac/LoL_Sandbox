public class CriticalStrikeChance : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    protected override string GetSimpleUIText()
    {
        return "CRITICAL STRIKE CHANCE: " + GetTotal() + "%";
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
