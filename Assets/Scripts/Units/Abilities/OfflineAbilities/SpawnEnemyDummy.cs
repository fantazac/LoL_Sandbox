public class SpawnEnemyDummy : SpawnDummy
{
    protected SpawnEnemyDummy()
    {
        abilityName = "Spawn Enemy Dummy";

        dummyResourcePath = "DummyPrefabs/EnemyDummy";
        team = Team.RED;
    }
}
