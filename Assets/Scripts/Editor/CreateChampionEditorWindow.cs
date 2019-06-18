using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateChampionEditorWindow : EditorWindow
{
    private string championName;
    private bool nameIsEmpty;
    private bool nameIsTaken;
    private string[] existingAbilities;
    private string[] existingAbilityTypes;
    private string[] abilityTypes;
    private List<string> existingChampions;

    private bool[] createNewAbilities;
    private int[] selectedExistingAbilities;

    private bool createNewStats;
    private float[] stats;
    private string[] statNames;

    private string championFileBasePath;
    private string championAbilityManagerFileBasePath;
    private string championBasicAttackFileBasePath;
    private string championBaseStatsFileBasePath;
    private string championStatsManagerFileBasePath;
    
    private string championAbilityPassiveTargetedFileBasePath;
    
    private string championFolderPath;
    private string championAbilitiesFolderPath;

    private CreateChampionEditorWindow()
    {
        championFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionFileBase.txt";
        championAbilityManagerFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionAbilityManagerFileBase.txt";
        championBasicAttackFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionBasicAttackFileBase.txt";
        championBaseStatsFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionBaseStatsFileBase.txt";
        championStatsManagerFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionStatsManagerFileBase.txt";
        
        championAbilityPassiveTargetedFileBasePath = "Assets/Scripts/Editor/BaseScripts/Abilities/ChampionAbilityPassiveTargetedFileBase.txt";

        championFolderPath = "Assets/Scripts/Entities/Units/MovingUnits/Character/Champion/";
        championAbilitiesFolderPath = "Assets/Scripts/Abilities/CharacterAbilities/";
        
        createNewAbilities = new bool[5];
        selectedExistingAbilities = new int[5];
        abilityTypes = new[] { "P", "Q", "W", "E", "R" };
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
    }

    [MenuItem ("Sandbox/Create Champion", false, 1)]
    private static void CreateChampion()
    {
        GetWindow<CreateChampionEditorWindow>("Create Champion");
    }

    private void OnEnable()
    {
        List<Type> existingAbilityTypesList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(a => a.IsSubclassOf(typeof(Ability)) && a.IsAbstract)
            .ToList();

        List<Type> existingAbilitiesTypeList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(a => a.IsSubclassOf(typeof(Ability)) && !a.IsAbstract)
            .ToList();
        
        var existingAbilitiesList = new List<string>();
        foreach (Type type in existingAbilitiesTypeList)
        {
            foreach (Type abilityType in existingAbilityTypesList)
            {
                if (type.IsSubclassOf(abilityType))
                {
                    existingAbilitiesList.Add(abilityType.Name + "/" + type.Name);
                }
            }
        }
        existingAbilitiesList.Sort();
        existingAbilities = existingAbilitiesList.ToArray();

        existingChampions = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(a => a.IsSubclassOf(typeof(Champion)) && !a.IsAbstract)
            .Select(a => a.Name)
            .ToList();
        existingChampions.Sort();
    }

    private void OnGUI()
    {
        GUILayout.Label("Champion Settings", EditorStyles.boldLabel);

        var newChampionName = EditorGUILayout.TextField("Champion name", championName);
        if (newChampionName != championName)
        {
            championName = newChampionName;
            nameIsEmpty = string.IsNullOrEmpty(championName);
            nameIsTaken = existingChampions.Contains(championName);
        }

        if (nameIsEmpty)
        {
            EditorGUILayout.HelpBox("Champion name field cannot be empty", MessageType.Error);   
        }

        if (nameIsTaken)
        {
            EditorGUILayout.HelpBox("Champion named '" + championName + "' already exists", MessageType.Error);   
        }

        var validName = !nameIsEmpty && !nameIsTaken;
        if (validName)
        {
            EditorGUILayout.HelpBox("Champion named '" + championName + "' will be created!", MessageType.Info);   
        }
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Abilities Settings", EditorStyles.boldLabel);
        GUILayout.Label("(Will use the selected existing ability for unchecked boxes)");
        EditorGUILayout.Separator();
        ShowAbilitySettings("P", 0);
        EditorGUILayout.Separator();
        ShowAbilitySettings("Q", 1);
        EditorGUILayout.Separator();
        ShowAbilitySettings("W", 2);
        EditorGUILayout.Separator();
        ShowAbilitySettings("E", 3);
        EditorGUILayout.Separator();
        ShowAbilitySettings("R", 4);
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
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
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUI.enabled = validName;
        if (GUILayout.Button("Create Champion"))
        {
            CreateChampionFiles();
            existingChampions.Add(championName);
            GUI.FocusControl("");
            championName = string.Empty;
            nameIsEmpty = true;
            AssetDatabase.Refresh();
        }
        GUI.enabled = true;
    }

    private void ShowAbilitySettings(string groupName, int groupId)
    {
        GUILayout.Label(groupName);
        if (GUILayout.Toggle(createNewAbilities[groupId], "Create new ability"))
        {
            createNewAbilities[groupId] = true;
        }
        else
        {
            createNewAbilities[groupId] = false;
            selectedExistingAbilities[groupId] = EditorGUILayout.Popup("Existing ability", selectedExistingAbilities[groupId], existingAbilities);
        }
    }
    
    private void CreateChampionFiles()
    {
        //Ability scripts
        
        var dirAbilitiesPath = championAbilitiesFolderPath + championName;
        if (!Directory.Exists(dirAbilitiesPath))
        {
            Directory.CreateDirectory(dirAbilitiesPath);
        }

        dirAbilitiesPath += "/" + championName;

        var abilityNames = new string[5];
        for (int i = 0; i < selectedExistingAbilities.Length; i++)
        {
            if (createNewAbilities[i])
            {
                abilityNames[i] = championName + "_" + abilityTypes[i];
                CreateAbilityFile(dirAbilitiesPath, championAbilityPassiveTargetedFileBasePath, abilityTypes[i]);
            }
            else
            {
                var abilityName = existingAbilities[selectedExistingAbilities[i]];
                abilityNames[i] = abilityName.Substring(abilityName.IndexOf('/') + 1);
            }
        }

        //Champion scripts
        
        var dirChampionPath = championFolderPath + championName;
        if (!Directory.Exists(dirChampionPath))
        {
            Directory.CreateDirectory(dirChampionPath);
        }

        dirChampionPath += "/" + championName;

        CreateAbilityManagerFile(dirChampionPath, championAbilityManagerFileBasePath, abilityNames);

        if (createNewStats)
        {
            CreateBaseStatsFile(dirChampionPath);
        }
        else
        {
            CreateChampionFile(dirChampionPath, championBaseStatsFileBasePath, "BaseStats");
        }
        
        CreateChampionFile(dirChampionPath, championBasicAttackFileBasePath, "BasicAttack");
        CreateChampionFile(dirChampionPath, championStatsManagerFileBasePath, "StatsManager");
        CreateChampionFile(dirChampionPath, championFileBasePath, "");
    }

    private void CreateAbilityFile(string filePath, string basePath, string abilityType)
    {
        filePath += "_" + abilityType + ".cs";
        string fileText = string.Format(File.ReadAllText(basePath), championName, abilityType);
        if (!File.Exists(filePath))
        {
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
    
    private void CreateAbilityManagerFile(string filePath, string basePath, string[] args)
    {
        filePath += "AbilityManager.cs";
        string fileText = string.Format(File.ReadAllText(basePath), championName, args[0], args[1], args[2], args[3], args[4]);
        if (!File.Exists(filePath))
        {
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }

    private void CreateBaseStatsFile(string filePath)
    {
        filePath += "BaseStats.cs";
        if (!File.Exists(filePath))
        {
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.WriteLine("public class " + championName + "BaseStats : CharacterBaseStats");
                outfile.WriteLine("{");
                outfile.WriteLine("    protected override void SetBaseStats()");
                outfile.WriteLine("    {");
                outfile.WriteLine("        BaseHealth = " + stats[0] + ";");
                outfile.WriteLine("        HealthPerLevel = " + stats[1] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseHealthRegeneration = " + stats[2] + ";");
                outfile.WriteLine("        HealthRegenerationPerLevel = " + stats[3] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseResource = " + stats[4] + ";");
                outfile.WriteLine("        ResourcePerLevel = " + stats[5] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseResourceRegeneration = " + stats[6] + ";");
                outfile.WriteLine("        ResourceRegenerationPerLevel = " + stats[7] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseAttackRange = " + stats[8] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseAttackDamage = " + stats[9] + ";");
                outfile.WriteLine("        AttackDamagePerLevel = " + stats[10] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseAttackSpeed = " + stats[11] + ";");
                outfile.WriteLine("        AttackSpeedPerLevel = " + stats[12] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseArmor = " + stats[13] + ";");
                outfile.WriteLine("        ArmorPerLevel = " + stats[14] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseMagicResistance = " + stats[15] + ";");
                outfile.WriteLine("        MagicResistancePerLevel = " + stats[16] + ";");
                outfile.WriteLine("");
                outfile.WriteLine("        BaseMovementSpeed = " + stats[17] + ";");
                outfile.WriteLine("    }");
                outfile.WriteLine("}");
            }
        }
    }
    
    private void CreateChampionFile(string filePath, string basePath, string fileType)
    {
        filePath += fileType + ".cs";
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
