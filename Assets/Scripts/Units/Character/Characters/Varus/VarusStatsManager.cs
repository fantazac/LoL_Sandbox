public class VarusStatsManager : ManaUserStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new VarusBaseStats();
    }
}
