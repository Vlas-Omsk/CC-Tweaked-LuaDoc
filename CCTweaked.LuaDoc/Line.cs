namespace CCTweaked.LuaDoc;

public sealed class Line
{
    public Line(LineType type, string data)
    {
        Type = type;
        Data = data;
    }

    public LineType Type { get; }
    public string Data { get; }
}
