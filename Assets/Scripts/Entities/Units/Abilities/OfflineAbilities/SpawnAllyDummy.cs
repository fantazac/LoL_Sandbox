public class SpawnAllyDummy : SpawnDummy
{
    protected SpawnAllyDummy()
    {
        maxDummyId = MAXIMUM_DUMMY_AMOUNT * 2;

        abilityName = "Spawn Ally Dummy";
        dummyName = "Ally Dummy";

        dummyResourcePath = "DummyPrefabs/AllyDummy";
        team = Team.BLUE;

        dummyId = MAXIMUM_DUMMY_AMOUNT;
    }
}
