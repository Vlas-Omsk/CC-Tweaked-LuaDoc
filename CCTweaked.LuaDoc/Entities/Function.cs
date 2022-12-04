namespace CCTweaked.LuaDoc.Entities;

public sealed class Function : Entity, IDefinition
{
    public string Name { get; set; }
    public ItemsOverload<Parameter>[] ParametersOverloads { get; set; }
    public ItemsOverload<Return>[] ReturnsOverloads { get; set; }
    public bool NeedSelf { get; set; }
}

public sealed class ItemsOverload<T>
{
    public T[] Items { get; set; }
}

public sealed class Parameter
{
    public string Name { get; set; }
    public bool Optional { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
}

public sealed class Return
{
    public string Type { get; set; }
    public string Description { get; set; }
}