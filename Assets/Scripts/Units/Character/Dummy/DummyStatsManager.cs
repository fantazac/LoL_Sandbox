public class DummyStatsManager : CharacterStatsManager
{
    //extra stats the character has that other characters don't

    protected override BaseStats GetBaseStats()
    {
        return new DummyBaseStats();
    }

    protected override void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        base.InitializeCharacterStats(characterBaseStats);

        //set extra character stats
    }
}
