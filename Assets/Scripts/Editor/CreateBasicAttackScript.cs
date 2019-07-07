using System.IO;

public class CreateBasicAttackScript : ICreateChampionScript
{
    private readonly string championBasicAttackFileBasePath;
    
    public CreateBasicAttackScript()
    {
        championBasicAttackFileBasePath = "Assets/Scripts/Editor/BaseScripts/ChampionBasicAttackFileBase.txt";
    }
    
    public void Init()
    {
    }

    public void ShowGUI()
    {
    }

    public void CreateScript(CreateScriptInfo info)
    {
        CreateBasicAttackFile(info.championName, info.path);
    }
    
    private void CreateBasicAttackFile(string championName, string filePath)
    {
        filePath += "BasicAttack.cs";
        if (!File.Exists(filePath))
        {
            string fileText = string.Format(File.ReadAllText(championBasicAttackFileBasePath), championName);
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(fileText);
            }
        }
    }
}
