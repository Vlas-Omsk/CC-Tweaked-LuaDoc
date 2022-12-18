namespace CCTweaked.LuaDoc;

public static class CCExtensions
{
    private static Dictionary<string, string> _interfaces = new Dictionary<string, string>()
    {
        { "colours", "colors" }
    };

    public static bool TryGetInterface(string baseName, out string @interface)
    {
        return _interfaces.TryGetValue(baseName, out @interface);
    }
}
