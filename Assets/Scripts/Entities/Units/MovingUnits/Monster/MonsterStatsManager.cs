public abstract class MonsterStatsManager : StatsManager
{
    //extra stats monsters have that other units don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeUnitStats(BaseStats baseStats)
    {
        base.InitializeUnitStats(baseStats);

        //set extra monster stats
    }
}