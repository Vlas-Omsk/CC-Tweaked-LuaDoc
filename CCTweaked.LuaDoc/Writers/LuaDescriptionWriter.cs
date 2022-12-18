using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Writers;

public sealed class LuaDescriptionWriter : DescriptionWriter
{
    public LuaDescriptionWriter(LuaWriter writer) : base(writer)
    {
    }

    protected override void WriteLinkNode(LinkNode linkNode)
    {
        FlushNewLines();

        switch (linkNode.Type)
        {
            case LinkNodeType.TypeLink:
                if (FormatLinkNodes)
                    Write("`");

                Write(linkNode.Link);

                if (FormatLinkNodes)
                    Write("`");
                break;
            case LinkNodeType.ExternalLink:
                Write($"[{linkNode.Name}]({linkNode.Link})");
                break;
            default:
                throw new InvalidDataException();
        }
    }
}
