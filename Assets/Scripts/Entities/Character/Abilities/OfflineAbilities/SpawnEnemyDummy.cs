using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyDummy : SpawnDummy
{
    protected SpawnEnemyDummy()
    {
        abilityName = "Spawn Enemy Dummy";

        dummyResourcePath = "Dummy/EnemyDummy";
        team = EntityTeam.RED;
    }
}
