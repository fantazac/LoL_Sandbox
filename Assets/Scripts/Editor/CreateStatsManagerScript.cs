using System.IO;

public class CreateStatsManagerScript : ICreateChampionScript
{
    private readonly string championStatsManagerFileBasePath;
    
    public CreateStatsManagerScript()
    {
        championStatsManagerFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionStatsManagerFileBase.txt";
    }
    
    public void Init()
    {
    }

    public void ShowGUI()
    {
    }

    public void CreateScript(CreateScriptInfo info)
    {
        CreateStatsManagerFile(info.championName, info.path);
    }
    
    private void CreateStatsManagerFile(string championName, string filePath)
    {
        filePath += "StatsManager.cs";
        if (!File.Exists(filePath))
        {
            string fileText = string.Format(File.ReadAllText(championStatsManagerFileBasePath), championName);
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
