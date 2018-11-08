public class TristanaStatsManager : ManaUserStatsManager
{
    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new TristanaBaseStats();
    }
}
