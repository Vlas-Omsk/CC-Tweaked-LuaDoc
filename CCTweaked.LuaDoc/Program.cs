using CCTweaked.LuaDoc.HtmlParser;
using CCTweaked.LuaDoc.Writers;

namespace CCTweaked.LuaDoc;

public static class Program
{
    private const string _modulesDirectory = "module";
    private const string _peripheralsDirectory = "peripheral";
    private const string _luaOutputPath = "cc_libs_lua";
    private const string _tsOutputPath = "cc_libs_ts";

    private static void Main(string[] args)
    {
        if (args.Length == 0 || !Directory.Exists(args[0]))
            throw new DirectoryNotFoundException();

        var htmlDocsDirectory = args[0];

        var files = GetFiles(Path.Combine(htmlDocsDirectory, _modulesDirectory))
            .Concat(GetFiles(Path.Combine(htmlDocsDirectory, _peripheralsDirectory)))
            .ToArray();

        GenerateTsDocs(files, htmlDocsDirectory);

        GenerateLuaDocs(files, htmlDocsDirectory);
    }

    private static void GenerateTsDocs(string[] files, string htmlDocsDirectory)
    {
        Directory.CreateDirectory(_tsOutputPath);

        using var indexWriter = new StreamWriter(Path.Combine(_tsOutputPath, "index.d.ts"));

        foreach (var filePath in files)
        {
            var relativeDirectory = Path.GetRelativePath(htmlDocsDirectory, Path.GetDirectoryName(filePath));

            var modules = new HtmlModulesParser(filePath, relativeDirectory).ParseModules();
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new TsWriter(Path.Combine(_tsOutputPath, fileName + ".d.ts"));

            var docWriter = new TsDocWriter(writer);
            docWriter.Write(modules);

            if (relativeDirectory[0] != '.')
                relativeDirectory = $"./{relativeDirectory}";

            indexWriter.WriteLine($"/// <reference path=\"{relativeDirectory}/{fileName}.d.ts\" />");
        }
    }

    private static void GenerateLuaDocs(string[] files, string htmlDocsDirectory)
    {
        Directory.CreateDirectory(_luaOutputPath);

        foreach (var filePath in files)
        {
            var relativeDirectory = Path.GetRelativePath(htmlDocsDirectory, Path.GetDirectoryName(filePath));

            var modules = new HtmlModulesParser(filePath, relativeDirectory).ParseModules();
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new LuaWriter(Path.Combine(_luaOutputPath, fileName + ".lua"));

            var docWriter = new LuaDocWriter(writer);
            docWriter.Write(modules);
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
