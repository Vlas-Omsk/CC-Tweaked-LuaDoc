using CCTweaked.LuaDoc.Html;
using CCTweaked.LuaDoc.Writers;

namespace CCTweaked.LuaDoc;

public static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0 || !Directory.Exists(args[0]))
            throw new DirectoryNotFoundException();

        var htmlDocsDirectory = args[0];

        GenerateTsDocs(htmlDocsDirectory);

        GenerateLuaDocs(htmlDocsDirectory);
    }

    private static void GenerateTsDocs(string htmlDocsDirectory)
    {
        Directory.CreateDirectory("cc_libs_ts");

        using var indexWriter = new StreamWriter(Path.Combine("cc_libs_ts", "index.d.ts"));

        foreach (var filePath in GetFiles(htmlDocsDirectory))
        {
            var modules = new HtmlModulesParser(filePath).ParseModules();
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new TsDocWriter(Path.Combine("cc_libs_ts", fileName + ".d.ts"));
            writer.Write(modules);

            var relativeDirectory = Path.GetRelativePath(htmlDocsDirectory, Path.GetDirectoryName(filePath));

            if (relativeDirectory[0] != '.')
                relativeDirectory = $"./{relativeDirectory}";

            indexWriter.WriteLine($"/// <reference path=\"{relativeDirectory}/{fileName}.d.ts\" />");
        }
    }

    private static void GenerateLuaDocs(string htmlDocsDirectory)
    {
        Directory.CreateDirectory("cc_libs_lua");

        foreach (var filePath in GetFiles(htmlDocsDirectory))
        {
            var modules = new HtmlModulesParser(filePath).ParseModules();
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new LuaDocWriter(Path.Combine("cc_libs_lua", fileName + ".lua"));
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
