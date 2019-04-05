using System.Collections.Generic;

public abstract class AutoTargeted : Ability, IAutoTargeted
{
    public virtual void UseAbility()
    {
        StartAbilityCast();

        StartCorrectCoroutine();
    }
    
    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }
}
