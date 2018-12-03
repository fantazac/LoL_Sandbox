public class Dummy : Character
{
    public DummyMovementManager DummyMovementManager { get; private set; }

    protected override void InitUnitProperties()
    {
        base.InitUnitProperties();

        DummyMovementManager = gameObject.AddComponent<DummyMovementManager>();
    }

    protected override void InitCharacterProperties()
    {
        StatsManager = gameObject.AddComponent<DummyStatsManager>();
    }

    public void SetDummyTeamAndID(Team team, int dummyId)
    {
        Team = team;
        ID = dummyId;
    }

    protected override MovementManager GetMovementManager()
    {
        return DummyMovementManager;
    }
}
