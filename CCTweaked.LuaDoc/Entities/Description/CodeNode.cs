namespace CCTweaked.LuaDoc.Entities.Description;

public sealed class CodeNode : IDescriptionNode
{
    public CodeNode(string content)
    {
        Content = content;
    }

    public string Content { get; }
}
