public class LucianStatsManager : ManaUserStatsManager
{
    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new LucianBaseStats();
    }
}
