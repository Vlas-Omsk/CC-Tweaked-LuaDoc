namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class AdmonitionNode : IDescriptionNode
{
    public AdmonitionNode(AdmonitionNodeType type, IDescriptionNode[] description)
    {
        Type = type;
        Description = description;
    }

    public AdmonitionNodeType Type { get; }
    public IDescriptionNode[] Description { get; }
}

public enum AdmonitionNodeType
{
    Tip,
    Note,
    Caution,
    Info
}