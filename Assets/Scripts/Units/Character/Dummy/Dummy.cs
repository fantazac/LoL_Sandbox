public class Dummy : Character
{
    protected override void SetCharacterSpecificScripts()
    {
        StatsManager = gameObject.AddComponent<DummyStatsManager>();
    }

    public void SetDummyTeamAndID(Team team, int dummyId)
    {
        Team = team;
        ID = dummyId;
    }
}
