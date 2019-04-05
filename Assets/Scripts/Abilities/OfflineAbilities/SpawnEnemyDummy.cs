public class SpawnEnemyDummy : SpawnDummy
{
    protected SpawnEnemyDummy()
    {
        maxDummyId = MAXIMUM_DUMMY_ID;

        abilityName = "Spawn Enemy Dummy";
        dummyName = "Enemy Dummy";

        dummyResourcePath = "DummyPrefabs/EnemyDummy";
        team = Team.RED;
    }
}
