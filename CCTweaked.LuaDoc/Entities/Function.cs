namespace CCTweaked.LuaDoc.Entities;

public sealed class Function : Entity, IDefinition
{
    public string Name { get; set; }
    public FunctionOverload<Parameter>[] ParametersOverloads { get; set; }
    public FunctionOverload<Return>[] ReturnsOverloads { get; set; }
    public bool NeedSelf { get; set; }
}