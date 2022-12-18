namespace CCTweaked.LuaDoc.Writers;

public sealed class LuaWriter : IWriter, IDisposable
{
    private readonly TextWriter _writer;
    private bool _isInComment;
    private bool _isCursorOnNewLine = true;

    public LuaWriter(string path) : this(new StreamWriter(path))
    {
    }

    public LuaWriter(TextWriter writer)
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
            if (_isInComment)
                _writer.Write("---");

            _isCursorOnNewLine = false;
        }

        if (str != null)
            _writer.Write(str);
    }

    public void EnterComment()
    {
        _isInComment = true;
    }

    public void ExitComment()
    {
        _isInComment = false;
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
