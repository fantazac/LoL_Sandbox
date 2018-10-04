public class CriticalStrikeChance : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }
}
