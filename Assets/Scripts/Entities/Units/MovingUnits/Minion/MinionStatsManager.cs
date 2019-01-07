public abstract class MinionStatsManager : StatsManager
{
    //extra stats minions have that other units don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializUnitStats(BaseStats baseStats)
    {
        base.InitializUnitStats(baseStats);

        //set extra minion stats
    }
}