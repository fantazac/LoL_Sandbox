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
    private List<string> existingChampions;

    private readonly string championFolderPath;

    private readonly List<ICreateChampionScript> scriptsToCreate;

    private CreateChampionEditorWindow()
    {
        championFolderPath = "Assets/Scripts/Entities/Units/MovingUnits/Character/Champion/";

        scriptsToCreate = new List<ICreateChampionScript>
        {
            new CreateAbilityManagerScript(),
            new CreateBaseStatsScript(),
            new CreateBasicAttackScript(),
            new CreateStatsManagerScript(),
            new CreateChampionScript()
        };
    }

    [MenuItem("Sandbox/Create Champion", false, 1)]
    private static void CreateChampion()
    {
        GetWindow<CreateChampionEditorWindow>("Create Champion");
    }

    private void OnEnable()
    {
        foreach (ICreateChampionScript script in scriptsToCreate)
        {
            script.Init();
        }

        existingChampions = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(a => a.IsSubclassOf(typeof(Champion)) && !a.IsAbstract)
            .Select(a => a.Name)
            .ToList();
        existingChampions.Sort();

        championName = string.Empty;
        nameIsEmpty = true;
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

        foreach (ICreateChampionScript script in scriptsToCreate)
        {
            script.ShowGUI();
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

    private void CreateChampionFiles()
    {
        var dirChampionPath = championFolderPath + championName;
        if (!Directory.Exists(dirChampionPath))
        {
            Directory.CreateDirectory(dirChampionPath);
        }

        dirChampionPath += "/" + championName;

        CreateScriptInfo info = new CreateScriptInfo(championName, dirChampionPath);
        foreach (ICreateChampionScript script in scriptsToCreate)
        {
            script.CreateScript(info);
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
