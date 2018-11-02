public class MissFortuneStatsManager : CharacterStatsManager
{
    //extra stats the character has that other characters don't

    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new MissFortuneBaseStats();
    }

    protected override void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        base.InitializeCharacterStats(characterBaseStats);

        ResourceType = ResourceType.MANA;
        Resource = new Resource(characterBaseStats.BaseResource, characterBaseStats.ResourcePerLevel);
        ResourceRegeneration = new ResourceRegeneration(characterBaseStats.BaseResourceRegeneration, characterBaseStats.ResourceRegenerationPerLevel);

        //set extra character stats
    }
}
