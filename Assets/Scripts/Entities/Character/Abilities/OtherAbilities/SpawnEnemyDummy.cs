using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyDummy : SpawnDummy
{
    protected SpawnEnemyDummy()
    {
        dummyResourceName = "_EnemyDummy";
        team = EntityTeam.RED;
    }
}
