public class Dummy : Character
{
    protected override void SetCharacterSpecificScripts()
    {
        EntityStats = gameObject.AddComponent<DummyStats>();
    }

    public void SetDummyTeamAndID(EntityTeam team, int dummyId)
    {
        Team = team;
        EntityId = dummyId;
    }
}
