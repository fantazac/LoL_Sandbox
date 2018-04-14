using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAllyDummy : SpawnDummy
{
    protected SpawnAllyDummy()
    {
        abilityName = "Spawn Ally Dummy";

        dummyResourceName = "_AllyDummy";
        team = EntityTeam.BLUE;
        dummyId = 5;
    }
}
