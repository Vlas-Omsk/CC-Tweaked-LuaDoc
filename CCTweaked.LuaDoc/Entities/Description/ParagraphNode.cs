namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class ParagraphNode : IDescriptionNode
{
    public ParagraphNode(ParagraphNodeType type, IDescriptionNode[] description)
    {
        Type = type;
        Description = description;
    }

    public ParagraphNodeType Type { get; }
    public IDescriptionNode[] Description { get; }
}

public enum ParagraphNodeType
{
    Default,
    Formatted
}