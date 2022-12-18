namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class ListNode : IDescriptionNode
{
    public ListNode(ListItemNode[] items)
    {
        Items = items;
    }

    public ListItemNode[] Items { get; }
}
