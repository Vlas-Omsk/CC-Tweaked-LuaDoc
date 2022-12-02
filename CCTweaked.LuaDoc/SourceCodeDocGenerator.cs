using CCTweaked.LuaDoc.Entities;
using CCTweaked.LuaDoc.SourceCode;

namespace CCTweaked.LuaDoc;

public sealed class SourceCodeDocGenerator
{
    public void Start()
    {
        var files = new Dictionary<string, List<string>>();

        MergeFiles(files, "/mnt/DATA/GitBuh/CC-Tweaked/doc/stub");
        MergeFiles(files, "/mnt/DATA/GitBuh/CC-Tweaked/build/docs/luaJavadoc");

        Directory.CreateDirectory("test");

        var linesReader = new SourceCodeLinesReader();
        var blockParser = new SourceCodeBlockParser();
        var entityParser = new SourceCodeEntityParser();

        foreach (var file in files)
        {
            IEnumerable<Entity> entities = null;

            foreach (var path in file.Value)
            {
                var linesBlock = linesReader.ReadLinesBlock(path);
                var blocks = linesBlock.Select(x => blockParser.Parse(x));

                if (entities == null)
                    entities = entityParser.Parse(blocks);
                else
                    entities = entities.Concat(entityParser.Parse(blocks));
            }

            using var entityWriter = new EntityWriter(Path.Combine("test", file.Key));

            entityWriter.Write(entities.ToArray());
        }
    }

    private static void MergeFiles(Dictionary<string, List<string>> files, string path)
    {
        foreach (var file in GetFiles(path))
        {
            var relativePath = Path.GetRelativePath(path, file);

            if (!files.TryGetValue(relativePath, out var paths))
                files.Add(relativePath, paths = new List<string>());

            paths.Add(file);
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
