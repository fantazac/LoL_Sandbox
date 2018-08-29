public class SpellVamp : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    protected override string GetSimpleUIText()
    {
        return "SPELL VAMP: " + GetTotal() + "%";
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
