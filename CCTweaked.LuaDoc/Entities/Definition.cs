namespace CCTweaked.LuaDoc.Entities;

public abstract class Definition : Entity
{
    protected Definition(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
