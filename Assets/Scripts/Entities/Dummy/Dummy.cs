using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    protected override void SetCharacterSpecificScripts()
    {
        EntityStats = gameObject.AddComponent<DummyStats>();
    }

    public void SetDummyTeamAndID(EntityTeam team, int dummyId)
    {
        Team = team;
        EntityId = dummyId;
    }
}
