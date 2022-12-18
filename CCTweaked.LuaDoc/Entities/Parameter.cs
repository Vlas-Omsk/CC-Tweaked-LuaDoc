using CCTweaked.LuaDoc.Entities.Description;

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
    public IDescriptionNode[] Description { get; set; }
    public string DefaultValue { get; set; }
}
