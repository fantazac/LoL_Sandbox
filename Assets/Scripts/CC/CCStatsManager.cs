public class CCStatsManager : ManaUserStatsManager
{
    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new CCBaseStats();
    }
}
