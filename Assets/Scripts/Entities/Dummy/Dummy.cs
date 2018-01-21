using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetDummyTeamAndID(EntityTeam team, int dummyId)
    {
        Team = team;
        EntityId = dummyId;
    }
}
