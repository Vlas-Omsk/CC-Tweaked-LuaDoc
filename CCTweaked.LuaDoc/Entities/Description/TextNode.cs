namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class TextNode : IDescriptionNode
{
    public TextNode(TextNodeStyle style, string content)
    {
        Style = style;
        Content = content;
    }

    public TextNodeStyle Style { get; }
    public string Content { get; }
}

public enum TextNodeStyle
{
    Normal,
    Bold,
    Italic,
    Header
}