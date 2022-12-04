using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public sealed class OverloadWithMergedReturns
{
    public OverloadWithMergedReturns(Parameter[] parameters, Return[][] returns)
    {
        Parameters = parameters;
        Returns = returns;
    }

    public Parameter[] Parameters { get; }
    public Return[][] Returns { get; }
}
