namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlSection
{
    public HtmlSection(HtmlSectionType type, object data)
    {
        Type = type;
        Data = data;
    }

    public HtmlSectionType Type { get; }
    public object Data { get; }
}

internal enum HtmlSectionType
{
    Parameters,
    Returns,
    SeeCollection
}