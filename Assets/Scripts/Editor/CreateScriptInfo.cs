public struct CreateScriptInfo
{
    public readonly string championName;
    public readonly string path;
    public readonly string extraInfo;

    public CreateScriptInfo(string championName, string path, string extraInfo = "")
    {
        this.championName = championName;
        this.path = path;
        this.extraInfo = extraInfo;
    }
}
