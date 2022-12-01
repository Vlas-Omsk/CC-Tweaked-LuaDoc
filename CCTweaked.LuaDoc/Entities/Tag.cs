namespace CCTweaked.LuaDoc.Entities;

public sealed class Tag
{
    public KeyValuePair<string, string>[] Params { get; set; }
    public string Data { get; set; }
}
