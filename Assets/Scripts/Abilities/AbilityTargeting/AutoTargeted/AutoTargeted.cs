using System.Collections.Generic;
using UnityEngine;

public abstract class AutoTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return Vector3.down; // This will change
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    public override bool CanBeCast(Unit target) { return false; }
    public override void UseAbility(Unit target) { }
}
