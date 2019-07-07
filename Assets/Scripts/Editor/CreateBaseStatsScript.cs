using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateBaseStatsScript : ICreateChampionScript
{
    private bool createNewStats;

    private readonly string championBaseStatsFileBasePath;
    private readonly float[] stats;
    private readonly string[] statNames;
    private readonly string[] baseStats;

    public CreateBaseStatsScript()
    {
        championBaseStatsFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionBaseStatsFileBase.txt";

        stats = new float[18];
        statNames = new[]
        {
            "Health",
            "Health/lvl",
            "HP regen",
            "HP regen/lvl",
            "Resource",
            "Resource/lvl",
            "Resource regen",
            "Resource regen/lvl",
            "Attack damage",
            "Attack damage/lvl",
            "Attack speed",
            "Attack speed/lvl",
            "Armor",
            "Armor/lvl",
            "Magic resist",
            "Magic resist/lvl",
            "Attack range",
            "Movement speed"
        };

        baseStats = new[]
        {
            "500", // Health
            "70", // Health per level
            "5", // Health regeneration
            "0.5f", // Health regeneration per level
            "350", // Resource
            "40", // Resource per level
            "5", // Resource regeneration
            "0.5f", // Resource regeneration per level
            "550", // Attack range
            "60", // Attack damage
            "2.5f", // Attack damage per level
            "0.625f", // Attack speed
            "1.5f", // Attack speed per level
            "30", // Armor
            "3", // Armor per level
            "30", // Magic resistance
            "0.5f", // Magic resistance per level
            "325" // Movement speed
        };
    }

    public void Init() { }

    public void ShowGUI()
    {
        GUILayout.Label("Stats Settings", EditorStyles.boldLabel);
        if (GUILayout.Toggle(createNewStats, "Create new stats (will use default stats if unchecked)"))
        {
            createNewStats = true;
            for (var i = 0; i < stats.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (i != 0)
                    {
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                }

                stats[i] = EditorGUILayout.FloatField(statNames[i], stats[i]);
            }

            GUILayout.EndHorizontal();
        }
        else
        {
            createNewStats = false;
        }
    }

    public void CreateScript(CreateScriptInfo info)
    {
        CreateBaseStatsFile(info.championName, info.path, createNewStats ? stats.ToDisplay() : baseStats);
    }

    private void CreateBaseStatsFile(string championName, string filePath, string[] selectedStats)
    {
        filePath += "BaseStats.cs";
        if (!File.Exists(filePath))
        {
            string fileText = string.Format(File.ReadAllText(championBaseStatsFileBasePath), championName, selectedStats[0], selectedStats[1], selectedStats[2],
                selectedStats[3], selectedStats[4], selectedStats[5], selectedStats[6], selectedStats[7], selectedStats[8], selectedStats[9], selectedStats[10],
                selectedStats[11], selectedStats[12], selectedStats[13], selectedStats[14], selectedStats[15], selectedStats[16], selectedStats[17]);
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
