public class PercentBonusOnlyStat : Stat
{
    protected override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}
