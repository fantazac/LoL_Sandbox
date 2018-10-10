public class SpawnAllyDummy : SpawnDummy
{
    protected SpawnAllyDummy()
    {
        abilityName = "Spawn Ally Dummy";

        dummyResourcePath = "DummyPrefabs/AllyDummy";
        team = EntityTeam.BLUE;
        dummyId = 5;
    }
}
