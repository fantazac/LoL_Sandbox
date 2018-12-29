using System;
using System.Collections.Generic;
using System.Linq;

public static class AffectedTeams
{
    public static List<Team> GetEnemyTeams(Team allyTeam)
    {
        List<Team> teams = GetTeams();
        teams.Remove(allyTeam);
        return teams;
    }

    public static List<Team> GetAllyTeam(Team allyTeam)
    {
        return new List<Team>() { allyTeam };
    }

    public static List<Team> GetTeams()
    {
        return Enum.GetValues(typeof(Team)).Cast<Team>().ToList();
    }
}
