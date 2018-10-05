public class DummyStats : CharacterStats
{
    //extra stats the character has that other characters don't

    protected override EntityBaseStats GetEntityBaseStats()
    {
        return new DummyBaseStats();
    }

    protected override void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        base.InitializeCharacterStats(characterBaseStats);

        //set extra character stats
    }
}
