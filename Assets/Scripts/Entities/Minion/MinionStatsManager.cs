public abstract class MinionStatsManager : EntityStatsManager
{
    //extra stats minions have that other entities don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeEntityStats(EntityBaseStats entityBaseStats)
    {
        base.InitializeEntityStats(entityBaseStats);

        //set extra minion stats
    }
}