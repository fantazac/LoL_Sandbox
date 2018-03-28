public class LifeSteal : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    public override string GetUIText()
    {
        return "LIFE STEAL: " + GetTotal() + "% (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
