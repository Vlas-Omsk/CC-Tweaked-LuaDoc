using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Entities;

public sealed class Return
{
    public string Type { get; set; }
    public IDescriptionNode[] Description { get; set; }
}
