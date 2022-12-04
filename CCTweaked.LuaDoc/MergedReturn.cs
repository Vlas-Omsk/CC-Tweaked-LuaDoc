using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public sealed class MergedReturn
{
    public MergedReturn(Return[] returns)
    {
        Returns = returns;
    }

    public Return[] Returns { get; }

    public string GetDescription()
    {
        return string.Join(" **or** ", Returns.Select(x =>
        {
            if (string.IsNullOrWhiteSpace(x.Description))
                return "<nothing>";

            return x.Description;
        }));
    }
}
