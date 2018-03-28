public class MagicPenetrationFlat : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    public override string GetUIText()
    {
        return "MAGIC PENETRATION FLAT: " + GetTotal() + " (" + GetBaseValue() + " + " + GetFlatBonus() + " - " + GetFlatMalus() + ")";
    }
}
