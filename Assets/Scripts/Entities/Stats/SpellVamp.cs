public class SpellVamp : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    public override string GetUIText()
    {
        return "SPELL VAMP: " + GetTotal() + "% (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
