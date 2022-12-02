namespace CCTweaked.LuaDoc.Entities;

public abstract class Entity
{
    public string Description { get; set; }
    public string[] See { get; set; }
    public string Source { get; set; }
}
