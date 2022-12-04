using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public sealed class MergedOverload
{
    public MergedOverload(Parameter[] parameters, Return[][] returns)
    {
        Parameters = parameters;
        Returns = returns;
    }

    public Parameter[] Parameters { get; }
    public Return[][] Returns { get; }
}
