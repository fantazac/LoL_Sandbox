public abstract class ManaUserStatsManager : ResourceUserStatsManager
{
    protected override void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        ResourceType = ResourceType.MANA;

        base.InitializeCharacterStats(characterBaseStats);
    }
}