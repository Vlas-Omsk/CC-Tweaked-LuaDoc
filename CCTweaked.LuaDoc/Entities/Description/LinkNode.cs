namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class LinkNode : IDescriptionNode
{
    public LinkNode(LinkNodeType type, string link, string name)
    {
        Type = type;
        Link = link;
        Name = name;
    }

    public LinkNodeType Type { get; }
    public string Link { get; }
    public string Name { get; }
}

public enum LinkNodeType
{
    TypeLink,
    ExternalLink
}