using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Writers;

public sealed class TsDescriptionWriter : DescriptionWriter
{
    public TsDescriptionWriter(TsWriter writer) : base(writer)
    {
    }

    protected override void WriteLinkNode(LinkNode linkNode)
    {
        FlushNewLines();

        switch (linkNode.Type)
        {
            case LinkNodeType.TypeLink:
            case LinkNodeType.ExternalLink:
                if (FormatLinkNodes)
                    Write($"{{@link {linkNode.Link} {linkNode.Name}}}");
                else
                    Write(linkNode.Link);
                break;
            default:
                throw new InvalidDataException();
        }
    }
}
