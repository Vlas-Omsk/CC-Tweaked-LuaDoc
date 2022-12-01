namespace CCTweaked.LuaDoc.Entities;

public sealed class Function : Entity
{
    public string Name { get; set; }
    public Overload[] Overloads { get; set; }
}

public sealed class Overload
{
    public Parameter[] Params { get; set; }
    public Return[] Returns { get; set; }
}

public sealed class Parameter
{
    public string Name { get; set; }
    public bool Optional { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}

public sealed class Return
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}