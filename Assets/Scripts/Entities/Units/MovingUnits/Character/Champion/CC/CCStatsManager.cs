public class CCStatsManager : ManaUserStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new CCBaseStats();
    }
}
