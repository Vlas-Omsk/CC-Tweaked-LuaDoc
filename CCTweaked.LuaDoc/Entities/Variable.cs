namespace CCTweaked.LuaDoc.Entities;

public sealed class Variable : Definition
{
    public Variable(string name) : base(name)
    {
    }

    public string Value { get; set; }
}
