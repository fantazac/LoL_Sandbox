using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    public void SetDummyTeamAndID(EntityTeam team, int dummyId)
    {
        Team = team;
        EntityId = dummyId;
    }
}
