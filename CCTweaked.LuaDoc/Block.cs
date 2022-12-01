namespace CCTweaked.LuaDoc;

public sealed class Block
{
    public Block(string description, KeyValuePair<string, Tag[]>[] tags, string[] data)
    {
        Description = description;
        Tags = tags;
        Data = data;
    }

    public string Description { get; }
    public KeyValuePair<string, Tag[]>[] Tags { get; }
    public string[] Data { get; }
}
