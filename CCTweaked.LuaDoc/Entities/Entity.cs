namespace CCTweaked.LuaDoc.Entities;

public abstract class Entity
{
    public string Description { get; set; }
    //public KeyValuePair<string, Tag[]>[] Tags { get; set; }
    public string[] See { get; set; }
}
