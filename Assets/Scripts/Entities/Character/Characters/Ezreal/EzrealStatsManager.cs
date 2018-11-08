public class EzrealStatsManager : ManaUserStatsManager
{
    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new EzrealBaseStats();
    }
}
