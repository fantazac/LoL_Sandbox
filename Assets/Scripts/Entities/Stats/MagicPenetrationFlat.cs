public class MagicPenetrationFlat : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    protected override string GetSimpleUIText()
    {
        return "MAGIC PENETRATION FLAT: " + GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " (" + GetBaseValue() + " + " + GetFlatBonus() + " - " + GetFlatMalus() + ")";
    }
}
