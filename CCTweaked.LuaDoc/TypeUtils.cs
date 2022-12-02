using System.Text.RegularExpressions;

namespace CCTweaked.LuaDoc;

public static class TypeUtils
{
    public static string NormalizeType(string type)
    {
        type = type.Replace("function(", "fun(");
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{x.Groups[1].Value}:");
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{{ [number]: {x.Groups[1].Value} }}");
        return type;
    }
}
