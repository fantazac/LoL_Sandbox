﻿public class SpawnEnemyDummy : SpawnDummy
{
    protected SpawnEnemyDummy()
    {
        maxDummyId = MAXIMUM_DUMMY_AMOUNT;

        abilityName = "Spawn Enemy Dummy";
        dummyName = "Enemy Dummy";

        dummyResourcePath = "DummyPrefabs/EnemyDummy";
        team = Team.RED;
    }
}
