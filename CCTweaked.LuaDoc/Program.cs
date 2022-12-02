using CCTweaked.LuaDoc.Html;
using CCTweaked.LuaDoc.Writers;

namespace CCTweaked.LuaDoc;

public static class Program
{
    private static void Main(string[] args)
    {
        Directory.CreateDirectory("cc_libs");

        foreach (var filePath in GetFiles("/mnt/DATA/GitBuh/CC-Tweaked/build/illuaminate/module"))
        {
            var modules = new HtmlModulesParser(filePath).ParseModules();
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new LuaDocWriter(Path.Combine("cc_libs", fileName + ".lua"));
            writer.Write(modules);
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
