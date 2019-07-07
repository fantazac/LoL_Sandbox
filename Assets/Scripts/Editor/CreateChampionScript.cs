using System.IO;

public class CreateChampionScript : ICreateChampionScript
{
    private readonly string championFileBasePath;
    
    public CreateChampionScript()
    {
        championFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionFileBase.txt";
    }
    
    public void Init()
    {
    }

    public void ShowGUI()
    {
    }

    public void CreateScript(CreateScriptInfo info)
    {
        CreateChampionFile(info.championName, info.path);
    }
    
    private void CreateChampionFile(string championName, string filePath)
    {
        filePath += ".cs";
        if (!File.Exists(filePath))
        {
            string fileText = string.Format(File.ReadAllText(championFileBasePath), championName);
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
