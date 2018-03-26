public class CriticalStrikeChance : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    public override string GetUIText()
    {
        return "CRITICAL STRIKE CHANCE: " + GetTotal() + "% (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
