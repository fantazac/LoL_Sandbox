public class MagicPenetrationFlat : Stat
{
    public MagicPenetrationFlat() : base() { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + flatBonus;
    }
}
