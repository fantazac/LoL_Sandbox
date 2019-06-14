using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateChampionEditorWindow : EditorWindow
{
    private string championName;

    private string championFileBasePath;
    private string championAbilityManagerFileBasePath;
    private string championBasicAttackFileBasePath;
    private string championBaseStatsFileBasePath;
    private string championStatsManagerFileBasePath;
    private string championFolderPath;

    private CreateChampionEditorWindow()
    {
        championFileBasePath = "Assets/Scripts/BaseScripts/ChampionFileBase.txt";
        championAbilityManagerFileBasePath = "Assets/Scripts/BaseScripts/ChampionAbilityManagerFileBase.txt";
        championBasicAttackFileBasePath = "Assets/Scripts/BaseScripts/ChampionBasicAttackFileBase.txt";
        championBaseStatsFileBasePath = "Assets/Scripts/BaseScripts/ChampionBaseStatsFileBase.txt";
        championStatsManagerFileBasePath = "Assets/Scripts/BaseScripts/ChampionStatsManagerFileBase.txt";
        championFolderPath = "Assets/Scripts/Entities/Units/MovingUnits/Character/Champion/";
    }

    [MenuItem ("Sandbox/Create Champion", false, 1)]
    private static void CreateChampion()
    {
        GetWindow<CreateChampionEditorWindow>("Create Champion");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Champion Settings", EditorStyles.boldLabel);
        championName = EditorGUILayout.TextField("Champion name", championName);
        if (GUILayout.Button("Create Champion"))
        {
            CreateChampionFile();
            AssetDatabase.Refresh();
        }
    }
    
    private void CreateChampionFile()
    {
        var dirPath = championFolderPath + championName;
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        dirPath += "/" + championName;

        CreateFile(dirPath + "AbilityManager.cs", championAbilityManagerFileBasePath);
        CreateFile(dirPath + "BaseStats.cs", championBaseStatsFileBasePath);
        CreateFile(dirPath + "BasicAttack.cs", championBasicAttackFileBasePath);
        CreateFile(dirPath + "StatsManager.cs", championStatsManagerFileBasePath);
        CreateFile(dirPath + ".cs", championFileBasePath);
    }

    private void CreateFile(string filePath, string basePath)
    {
        string fileText = string.Format(File.ReadAllText(basePath), championName);
        if (!File.Exists(filePath))
        {
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
