using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateAbilityManagerScript : ICreateChampionScript
{
    private string[] existingAbilities;

    private readonly string championAbilityManagerFileBasePath;
    private readonly string championAbilitiesFolderPath;

    private readonly bool[] createNewAbilities;
    private readonly int[] selectedExistingAbilities;
    private readonly string[] abilityTypes;
    private readonly string[] abilityGroupNames;

    private readonly ICreateChampionScript abilityScriptToCreate;

    public CreateAbilityManagerScript()
    {
        championAbilityManagerFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionAbilityManagerFileBase.txt";
        championAbilitiesFolderPath = "Assets/Scripts/Abilities/CharacterAbilities/";

        createNewAbilities = new bool[5];
        selectedExistingAbilities = new int[5];
        abilityTypes = new[] { "P", "Q", "W", "E", "R" };
        abilityGroupNames = new[] { "P", "Q", "W", "E", "R" };

        abilityScriptToCreate = new CreateAbilityScript();
    }

    public void Init()
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

        abilityScriptToCreate.Init();
    }

    public void ShowGUI()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Abilities Settings", EditorStyles.boldLabel);
        GUILayout.Label("(Will use the selected existing ability for unchecked boxes)");
        for (int i = 0; i < abilityGroupNames.Length; i++)
        {
            EditorGUILayout.Separator();
            ShowAbilitySettings(abilityGroupNames[i], i);
        }
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

    public void CreateScript(CreateScriptInfo info)
    {
        var championName = info.championName;
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
                abilityScriptToCreate.CreateScript(new CreateScriptInfo(championName, dirAbilitiesPath, abilityTypes[i]));
            }
            else
            {
                var abilityName = existingAbilities[selectedExistingAbilities[i]];
                abilityNames[i] = abilityName.Substring(abilityName.IndexOf('/') + 1);
            }
        }

        CreateAbilityManagerFile(championName, info.path, abilityNames);
    }

    private void CreateAbilityManagerFile(string championName, string filePath, string[] abilityNames)
    {
        filePath += "AbilityManager.cs";
        if (!File.Exists(filePath))
        {
            string fileText = string.Format(File.ReadAllText(championAbilityManagerFileBasePath), championName, abilityNames[0], abilityNames[1], abilityNames[2],
                abilityNames[3], abilityNames[4]);
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
