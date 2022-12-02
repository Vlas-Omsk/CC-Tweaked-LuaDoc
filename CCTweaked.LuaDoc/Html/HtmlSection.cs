namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlSection
{
    public HtmlSection(SectionType type, object data)
    {
        Type = type;
        Data = data;
    }

    public SectionType Type { get; }
    public object Data { get; }
}

public enum SectionType
{
    Parameters,
    Returns,
    SeeCollection
}