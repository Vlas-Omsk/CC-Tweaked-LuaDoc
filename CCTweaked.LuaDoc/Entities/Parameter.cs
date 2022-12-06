namespace CCTweaked.LuaDoc.Entities;

public sealed class Parameter
{
    public Parameter(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public bool Optional { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
}
