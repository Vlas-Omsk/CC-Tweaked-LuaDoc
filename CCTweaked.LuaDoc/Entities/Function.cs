namespace CCTweaked.LuaDoc.Entities;

public sealed class Function : Entity, IDefinition
{
    public string Name { get; set; }
    public Overload<Parameter>[] ParametersOverloads { get; set; }
    public Overload<Return>[] ReturnsOverloads { get; set; }
}

public sealed class Overload<T>
{
    public T[] Overloads { get; set; }
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
    public string Type { get; set; }
    public string Description { get; set; }
}