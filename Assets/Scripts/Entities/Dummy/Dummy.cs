public class Dummy : Character
{
    protected override void SetCharacterSpecificScripts()
    {
        EntityStatsManager = gameObject.AddComponent<DummyStatsManager>();
    }

    public void SetDummyTeamAndID(EntityTeam team, int dummyId)
    {
        Team = team;
        EntityId = dummyId;
    }
}
