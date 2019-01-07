public class MissFortuneStatsManager : ManaUserStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new MissFortuneBaseStats();
    }
}
