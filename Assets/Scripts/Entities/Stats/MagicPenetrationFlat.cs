public class MagicPenetrationFlat : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }
}
