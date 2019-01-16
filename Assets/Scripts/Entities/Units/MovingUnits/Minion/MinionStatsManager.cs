public abstract class MinionStatsManager : StatsManager
{
    //extra stats minions have that other units don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeUnitStats(BaseStats baseStats)
    {
        base.InitializeUnitStats(baseStats);

        //set extra minion stats
    }
}