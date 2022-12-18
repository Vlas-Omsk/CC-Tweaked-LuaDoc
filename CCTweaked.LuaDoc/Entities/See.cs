using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Entities;

public sealed class See
{
    public LinkNode Link { get; set; }
    public IDescriptionNode[] Description { get; set; }
}
