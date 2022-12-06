namespace CCTweaked.LuaDoc.Entities;

public sealed class Function : Definition
{
    public Function(string name, bool needSelf) : base(name)
    {
        NeedSelf = needSelf;
    }

    public bool NeedSelf { get; }
    public FunctionOverload<Parameter>[] ParametersOverloads { get; set; }
    public FunctionOverload<Return>[] ReturnsOverloads { get; set; }
}