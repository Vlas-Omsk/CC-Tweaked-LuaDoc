using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public sealed class Overload<T>
{
    public Overload(Parameter[] parameters, T[] returns)
    {
        Parameters = parameters;
        Returns = returns;
    }

    public Parameter[] Parameters { get; }
    public T[] Returns { get; }
}
