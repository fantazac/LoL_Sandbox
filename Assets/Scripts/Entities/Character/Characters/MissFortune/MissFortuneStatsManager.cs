public class MissFortuneStatsManager : ManaUserStatsManager
{
    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new MissFortuneBaseStats();
    }
}
