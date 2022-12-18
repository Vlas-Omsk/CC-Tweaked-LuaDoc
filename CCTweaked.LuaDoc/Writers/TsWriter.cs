namespace CCTweaked.LuaDoc.Writers;

public sealed class TsWriter : IWriter, IDisposable
{
    private readonly TextWriter _writer;
    private int _indent;
    private bool _comment;
    private bool _isCursorOnNewLine = true;

    public TsWriter(string path) : this(new StreamWriter(path))
    {
    }

    public TsWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void WriteLine(string str)
    {
        Write(str);
        _writer.WriteLine();
        _isCursorOnNewLine = true;
    }

    public void Write(string str)
    {
        if (str == null)
        {
            WriteInternal(str);
            return;
        }

        var lines = str.Split(Environment.NewLine);

        for (var i = 0; i < lines.Length - 1; i++)
        {
            WriteInternal(lines[i]);
            _writer.WriteLine();
            _isCursorOnNewLine = true;
        }

        WriteInternal(lines[lines.Length - 1]);
    }

    private void WriteInternal(string str)
    {
        if (_isCursorOnNewLine)
        {
            if (_indent > 0)
                _writer.Write(GetIndent());
            if (_comment)
                _writer.Write(" * ");

            _isCursorOnNewLine = false;
        }

        if (str != null)
        {
            if (_comment)
                str = str.Replace("*/", "*⁠/");

            _writer.Write(str);
        }
    }

    private string GetIndent()
    {
        return new string(' ', _indent * 2);
    }

    public void EnterComment()
    {
        _comment = true;
        _writer.WriteLine(GetIndent() + "/**");
    }

    public void ExitComment()
    {
        _comment = false;
        _writer.WriteLine(GetIndent() + " */");
    }

    public void IncreaseIndent()
    {
        _indent++;
    }

    public void DecreaseIndent()
    {
        if (_indent == 0)
            throw new Exception();

        _indent--;
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
