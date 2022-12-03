using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc.Writers;

public sealed class TsDocWriter : IDocWriter, IDisposable
{
    private const int _threashold = 80;
    private readonly TextWriter _writer;

    public TsDocWriter(string path) : this(new StreamWriter(path))
    {
    }

    public TsDocWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(IEnumerable<Module> modules)
    {
        using var enumerator = modules.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new Exception();

        WriteBaseModule(enumerator.Current);

        while (enumerator.MoveNext())
        {
            WriteTypeModule(enumerator.Current);
        }
    }

    private void WriteBaseModule(Module module)
    {
        if (module.IsType)
            throw new Exception();

        _writer.WriteLine($"declare namespace {module.Name} {{");
        _writer.WriteLine("}");
        _writer.WriteLine();
    }

    private void WriteTypeModule(Module module)
    {
        if (!module.IsType)
            throw new Exception();

        _writer.WriteLine($"declare interface {module.Name} {{");
        _writer.WriteLine("}");
        _writer.WriteLine();
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
