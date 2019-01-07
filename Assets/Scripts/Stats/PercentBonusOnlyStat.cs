public class PercentBonusOnlyStat : Stat
{
    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}
