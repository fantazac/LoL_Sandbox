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

    private bool[] useExistingAbilities;
    private int[] selectedExistingAbilities;

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
        
        useExistingAbilities = new bool[5];
        selectedExistingAbilities = new int[5];
        abilityTypes = new[] { "P", "Q", "W", "E", "R" };
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

        GUI.enabled = validName;
        if (GUILayout.Button("Create Champion"))
        {
            CreateChampionFile();
            AssetDatabase.Refresh();
        }
        GUI.enabled = true;
    }

    private void ShowAbilitySettings(string groupName, int groupId)
    {
        GUILayout.Label(groupName);
        if (GUILayout.Toggle(useExistingAbilities[groupId], "Use existing ability"))
        {
            useExistingAbilities[groupId] = true;
            selectedExistingAbilities[groupId] = EditorGUILayout.Popup("Existing ability", selectedExistingAbilities[groupId], existingAbilities);
        }
        else
        {
            useExistingAbilities[groupId] = false;
        }
    }
    
    private void CreateChampionFile()
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
            if (useExistingAbilities[i])
            {
                var abilityName = existingAbilities[selectedExistingAbilities[i]];
                abilityNames[i] = abilityName.Substring(abilityName.IndexOf('/') + 1);
            }
            else
            {
                abilityNames[i] = championName + "_" + abilityTypes[i];
                CreateAbilityFile(dirAbilitiesPath, championAbilityPassiveTargetedFileBasePath, abilityTypes[i]);
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
        
        CreateChampionFile(dirChampionPath, championBaseStatsFileBasePath, "BaseStats");
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
