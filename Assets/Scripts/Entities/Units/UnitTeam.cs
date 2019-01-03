using System;
using System.Collections.Generic;
using System.Linq;

public enum Team
{
    BLUE,
    RED,
    NEUTRAL
}

public static class TeamMethods
{
    public static List<Team> GetAllyTeam(Team allyTeam)
    {
        return new List<Team>() { allyTeam };
    }

    public static List<Team> GetCharacterTeams()
    {
        List<Team> characterTeams = GetTeams();
        characterTeams.Remove(Team.NEUTRAL);
        return characterTeams;
    }

    public static List<Team> GetEnemyTeams(Team allyTeam)
    {
        List<Team> enemyTeams = GetHostileTeams(allyTeam);
        if (allyTeam != Team.NEUTRAL)
        {
            enemyTeams.Remove(Team.NEUTRAL);
        }
        return enemyTeams;
    }

    public static List<Team> GetHostileTeams(Team allyTeam)
    {
        List<Team> hostileTeams = GetTeams();
        hostileTeams.Remove(allyTeam);
        return hostileTeams;
    }

    public static List<Team> GetTeams()
    {
        return Enum.GetValues(typeof(Team)).Cast<Team>().ToList();
    }
}
