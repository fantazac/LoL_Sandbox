public class LucianStatsManager : ManaUserStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new LucianBaseStats();
    }
}
