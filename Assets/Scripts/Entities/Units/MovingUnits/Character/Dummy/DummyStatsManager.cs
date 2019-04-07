public class DummyStatsManager : CharacterStatsManager
{
    protected override BaseStats GetBaseStats()
    {
        return new DummyBaseStats();
    }
}
