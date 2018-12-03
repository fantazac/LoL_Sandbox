public class TristanaStatsManager : ManaUserStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new TristanaBaseStats();
    }
}
