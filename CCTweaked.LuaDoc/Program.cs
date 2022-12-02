using CCTweaked.LuaDoc.Html;

namespace CCTweaked.LuaDoc;

public static class Program
{
    private static void Main(string[] args)
    {
        new HtmlParser("/mnt/DATA/GitBuh/CC-Tweaked/build/illuaminate/module/_G.html").Start();
    }
}
