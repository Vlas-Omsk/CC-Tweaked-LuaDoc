namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class ListItemNode
{
    public ListItemNode(IDescriptionNode[] description)
    {
        Description = description;
    }

    public IDescriptionNode[] Description { get; }
}
