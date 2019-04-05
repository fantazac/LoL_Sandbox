public class SpawnAllyDummy : SpawnDummy
{
    protected SpawnAllyDummy()
    {
        maxDummyId = MAXIMUM_DUMMY_ID * 2;
        minDummyId = MAXIMUM_DUMMY_ID;

        abilityName = "Spawn Ally Dummy";
        dummyName = "Ally Dummy";

        dummyResourcePath = "DummyPrefabs/AllyDummy";
        team = Team.BLUE;

        dummyId = minDummyId;
    }
}
