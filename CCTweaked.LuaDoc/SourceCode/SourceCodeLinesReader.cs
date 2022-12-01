using CCTweaked.LuaDoc.SourceCode.Entities;

namespace CCTweaked.LuaDoc.SourceCode;

public sealed class SourceCodeLinesReader
{
    private string _line;

    public SourceCodeLinesReader()
    {
    }

    public IEnumerable<Line[]> ReadLinesBlock(string path)
    {
        using var reader = new StreamReader(path);
        _line = reader.ReadLine();

        while (_line != null)
            yield return ReadLines(reader).ToArray();
    }

    private IEnumerable<Line> ReadLines(StreamReader reader)
    {
        while (string.IsNullOrEmpty(_line))
            _line = reader.ReadLine();

        do
        {
            var line = _line;

            if (line.StartsWith("--[[-"))
            {
                line = line[5..];

                do
                {
                    var isEndOfComment = false;

                    if (line.EndsWith("]]"))
                    {
                        line = line[..(line.Length - 2)];
                        isEndOfComment = true;
                    }

                    line = line.Trim();

                    yield return new Line(LineType.Comment, line);

                    if (isEndOfComment)
                        break;
                }
                while ((line = reader.ReadLine()) != null);
            }
            else if (TryGetCommentText(line, out var text))
            {
                yield return new Line(LineType.Comment, text);
            }
            else
            {
                if (!string.IsNullOrEmpty(line))
                {
                    yield return new Line(LineType.Data, line);
                }
                else
                {
                    _line = reader.ReadLine();
                    break;
                }
            }
        }
        while ((_line = reader.ReadLine()) != null);
    }

    private static bool TryGetCommentText(string line, out string text)
    {
        if (line.StartsWith("---"))
        {
            line = line[3..];
        }
        else if (line.StartsWith("--"))
        {
            line = line[2..];
        }
        else
        {
            text = null;
            return false;
        }


        text = line.Trim();
        return true;
    }
}
