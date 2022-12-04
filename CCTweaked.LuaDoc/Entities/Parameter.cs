namespace CCTweaked.LuaDoc.Entities;

public sealed class Parameter
{
    public string Name { get; set; }
    public bool Optional { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
}
