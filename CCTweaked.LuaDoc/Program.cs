using CCTweaked.LuaDoc.Html;

namespace CCTweaked.LuaDoc;

public static class Program
{
    private static void Main(string[] args)
    {
        foreach (var file in GetFiles("/mnt/DATA/GitBuh/CC-Tweaked/build/illuaminate/module"))
        {
            var arrays = new HtmlModulesParser(file).ParseModules().ToArray();
        }
    }

    private static IEnumerable<string> GetFiles(string path)
    {
        foreach (var filePath in Directory.GetFiles(path))
            yield return filePath;
        foreach (var directoryPath in Directory.GetDirectories(path))
            foreach (var filePath in GetFiles(directoryPath))
                yield return filePath;
    }
}
