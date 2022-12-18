using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Entities;

public abstract class Entity
{
    public IDescriptionNode[] Description { get; set; }
    public See[] See { get; set; }
    public string Source { get; set; }
}
