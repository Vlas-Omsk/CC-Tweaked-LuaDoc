namespace CCTweaked.LuaDoc.Entities;

public sealed class Module : Entity
{
    public string Name { get; set; }
    public IDefinition[] Definitions { get; set; }
    public bool IsType { get; set; }
}
