using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAllyDummy : SpawnDummy
{
    protected SpawnAllyDummy()
    {
        abilityName = "Spawn Ally Dummy";

        dummyResourcePath = "Dummy/AllyDummy";
        team = EntityTeam.BLUE;
        dummyId = 5;
    }
}
