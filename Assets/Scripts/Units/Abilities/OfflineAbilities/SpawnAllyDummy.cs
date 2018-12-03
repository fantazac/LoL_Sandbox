public class SpawnAllyDummy : SpawnDummy
{
    protected SpawnAllyDummy()
    {
        abilityName = "Spawn Ally Dummy";

        dummyResourcePath = "DummyPrefabs/AllyDummy";
        team = Team.BLUE;
        dummyId = 5;
    }
}
