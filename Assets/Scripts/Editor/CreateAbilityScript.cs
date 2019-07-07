using System.IO;

public class CreateAbilityScript : ICreateChampionScript
{
    private readonly string championAbilityPassiveTargetedFileBasePath;

    public CreateAbilityScript()
    {
        championAbilityPassiveTargetedFileBasePath = "Assets/Scripts/Editor/BaseScripts/Abilities/ChampionAbilityPassiveTargetedFileBase.txt";
    }

    public void Init()
    {
    }

    public void ShowGUI()
    {
    }

    public void CreateScript(CreateScriptInfo info)
    {
        CreateAbilityFile(info.championName, info.path, info.extraInfo);
    }

    private void CreateAbilityFile(string championName, string filePath, string abilityType)
    {
        filePath += "_" + abilityType + ".cs";
        if (!File.Exists(filePath))
        {
            string fileText = string.Format(File.ReadAllText(championAbilityPassiveTargetedFileBasePath), championName, abilityType);
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
