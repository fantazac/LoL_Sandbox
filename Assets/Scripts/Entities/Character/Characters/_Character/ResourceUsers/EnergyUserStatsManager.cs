public abstract class EnergyUserStatsManager : ResourceUserStatsManager
{
    protected override void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        ResourceType = ResourceType.ENERGY;

        base.InitializeCharacterStats(characterBaseStats);
    }
}
