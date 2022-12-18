using CCTweaked.LuaDoc.Entities;
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
        var modulesCache = new Dictionary<string, Module[]>();
        var modulesToWrite = new Dictionary<string, Module[]>();

        Directory.CreateDirectory(_tsOutputPath);

        using var indexWriter = new StreamWriter(Path.Combine(_tsOutputPath, "index.d.ts"));

        foreach (var filePath in files)
        {
            var relativeDirectory = Path.GetRelativePath(htmlDocsDirectory, Path.GetDirectoryName(filePath));

            Directory.CreateDirectory(Path.Combine(_tsOutputPath, relativeDirectory));

            var modules = new HtmlModulesParser(filePath, relativeDirectory).ParseModules().ToArray();

            if (modules[0].Type != ModuleType.Module)
                throw new Exception();

            var extends = Array.Empty<Module>();

            modulesCache.Add(modules[0].Name, modules);

            if (CCExtensions.TryGetInterface(modules[0].Name, out string @interface))
            {
                if (!modulesCache.TryGetValue(@interface, out extends))
                {
                    modulesToWrite.Add(filePath, modules);
                    continue;
                }
            }

            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new TsWriter(Path.Combine(_tsOutputPath, relativeDirectory, fileName + ".d.ts"));

            var docWriter = new TsDocWriter(writer);
            docWriter.Write(modules, extends);

            if (relativeDirectory[0] != '.')
                relativeDirectory = $"./{relativeDirectory}";

            indexWriter.WriteLine($"/// <reference path=\"{relativeDirectory}/{fileName}.d.ts\" />");
        }

        while (modulesToWrite.Count > 0)
        {
            foreach (var keyValue in modulesToWrite)
            {
                var relativeDirectory = Path.GetRelativePath(htmlDocsDirectory, Path.GetDirectoryName(keyValue.Key));

                if (CCExtensions.TryGetInterface(keyValue.Value[0].Name, out string @interface))
                {
                    if (!modulesCache.ContainsKey(@interface))
                        continue;
                }

                var fileName = Path.GetFileNameWithoutExtension(keyValue.Key);

                using var writer = new TsWriter(Path.Combine(_tsOutputPath, relativeDirectory, fileName + ".d.ts"));

                var docWriter = new TsDocWriter(writer);
                docWriter.Write(keyValue.Value, modulesCache[@interface]);

                if (relativeDirectory[0] != '.')
                    relativeDirectory = $"./{relativeDirectory}";

                indexWriter.WriteLine($"/// <reference path=\"{relativeDirectory}/{fileName}.d.ts\" />");
            }
        }
    }

    private static void GenerateLuaDocs(string[] files, string htmlDocsDirectory)
    {
        foreach (var filePath in files)
        {
            var relativeDirectory = Path.GetRelativePath(htmlDocsDirectory, Path.GetDirectoryName(filePath));

            Directory.CreateDirectory(Path.Combine(_luaOutputPath, relativeDirectory));

            var modules = new HtmlModulesParser(filePath, relativeDirectory).ParseModules();
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using var writer = new LuaWriter(Path.Combine(_luaOutputPath, relativeDirectory, fileName + ".lua"));

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
