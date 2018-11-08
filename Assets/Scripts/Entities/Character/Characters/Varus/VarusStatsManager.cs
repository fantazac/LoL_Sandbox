public class VarusStatsManager : ManaUserStatsManager
{
    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new VarusBaseStats();
    }
}
