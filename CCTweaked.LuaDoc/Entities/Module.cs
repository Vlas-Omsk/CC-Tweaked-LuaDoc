namespace CCTweaked.LuaDoc.Entities;

public sealed class Module : Entity
{
    public Module(string name, ModuleType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public ModuleType Type { get; }
    public Definition[] Definitions { get; set; }
}

public enum ModuleType
{
    Module,
    Type
}
