public abstract class MonsterStatsManager : StatsManager
{
    //extra stats monsters have that other entities don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeEntityStats(EntityBaseStats entityBaseStats)
    {
        base.InitializeEntityStats(entityBaseStats);

        //set extra monster stats
    }
}