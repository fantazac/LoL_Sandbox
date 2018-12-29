public class Dummy : Character
{
    public DummyMovementManager DummyMovementManager { get; private set; }

    protected override void InitUnitProperties()
    {
        base.InitUnitProperties();

        DummyMovementManager = gameObject.AddComponent<DummyMovementManager>();
        StatusManager = gameObject.AddComponent<DummyStatusManager>();
    }

    protected override void InitCharacterProperties()
    {
        StatsManager = gameObject.AddComponent<DummyStatsManager>();
    }

    public void SetDummyNameAndTeamAndID(string dummyName, Team team, int dummyId)
    {
        Name = dummyName;
        Team = team;
        SetID(dummyId);
    }

    protected override MovementManager GetMovementManager()
    {
        return DummyMovementManager;
    }
}
