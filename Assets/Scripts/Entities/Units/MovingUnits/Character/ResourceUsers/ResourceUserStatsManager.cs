public abstract class ResourceUserStatsManager : CharacterStatsManager
{
    protected override void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        base.InitializeCharacterStats(characterBaseStats);

        Resource = new Resource(characterBaseStats.BaseResource, characterBaseStats.ResourcePerLevel);
        ResourceRegeneration = new ResourceRegeneration(characterBaseStats.BaseResourceRegeneration, characterBaseStats.ResourceRegenerationPerLevel);
    }
}
