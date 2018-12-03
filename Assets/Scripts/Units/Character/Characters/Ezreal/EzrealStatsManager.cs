public class EzrealStatsManager : ManaUserStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new EzrealBaseStats();
    }
}
