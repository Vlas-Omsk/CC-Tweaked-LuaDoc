namespace CCTweaked.LuaDoc.Entities;

public sealed class FunctionOverload<T>
{
    public FunctionOverload(T[] items)
    {
        Items = items;
    }

    public T[] Items { get; }
}
