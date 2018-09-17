public class MissFortuneStats : CharacterStats
{
    //extra stats the character has that other characters don't

    protected override void Awake()
    {
        ResourceType = ResourceType.MANA;

        base.Awake();
    }

    protected override EntityBaseStats SetEntityBaseStats()
    {
        return gameObject.AddComponent<MissFortuneBaseStats>();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra character stats
    }
}
