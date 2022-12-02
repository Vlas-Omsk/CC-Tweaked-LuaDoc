namespace CCTweaked.LuaDoc.Entities;

public sealed class Variable : Entity, IDefinition
{
    public string Name { get; set; }
    public string Value { get; set; }
}
