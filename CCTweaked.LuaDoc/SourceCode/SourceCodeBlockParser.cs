using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;
using CCTweaked.LuaDoc.SourceCode.Entities;

namespace CCTweaked.LuaDoc.SourceCode;

public sealed class SourceCodeBlockParser
{
    private Line[] _block;
    private int _index = 0;
    private string _description;
    private readonly Dictionary<string, List<Tag>> _tags = new Dictionary<string, List<Tag>>();
    private readonly List<string> _data = new List<string>();

    public SourceCodeBlockParser()
    {
    }

    public Block Parse(Line[] block)
    {
        _block = block;

        while (_index < _block.Length)
            ParseLine();

        var blockEntity = new Block(_description, _tags.Select(x => new KeyValuePair<string, Tag[]>(x.Key, x.Value.ToArray())).ToArray(), _data.ToArray());

        _index = 0;
        _description = null;
        _tags.Clear();
        _data.Clear();

        return blockEntity;
    }

    private void ParseLine()
    {
        var line = _block[_index];

        if (line.Type == LineType.Comment)
        {
            if (IsTagStart(line.Data))
                ParseTag(line);
            else
                ParseDescription(line);
        }
        else
        {
            _data.Add(line.Data);
            _index++;
        }
    }

    private void ParseTag(Line line)
    {
        var match = Regex.Match(line.Data, @"@([a-zA-Z]*)(\[(.*?)\])?\s*(.*)");

        var tagName = match.Groups[1].Value;
        var paramsMatches = Regex.Matches(match.Groups[3].Value, @"([a-zA-Z0-9]+)(\s*=\s*(.*))?");
        var data = match.Groups[4].Value + ParseText();

        var @params = new Dictionary<string, string>();

        foreach (Match paramMatch in paramsMatches)
            @params.Add(paramMatch.Groups[1].Value, paramMatch.Groups[3].Value);

        var tag = new Tag()
        {
            Params = @params.ToArray(),
            Data = data
        };

        if (!_tags.TryGetValue(tagName, out var tags))
            _tags[tagName] = new List<Tag>() { tag };
        else
            tags.Add(tag);
    }

    private void ParseDescription(Line line)
    {
        if (_description != null)
            throw new Exception("Internal error");

        _description = line.Data + ParseText();
    }

    private string ParseText()
    {
        Line line;
        string text = string.Empty;
        bool joining = false;
        bool lastIsNewLine = false;

        while (++_index < _block.Length)
        {
            line = _block[_index];

            if (line.Type != LineType.Comment || IsTagStart(line.Data))
                break;

            if (string.IsNullOrEmpty(line.Data))
            {
                text += Environment.NewLine;
                joining = false;
                lastIsNewLine = true;
            }
            else
            {
                if (lastIsNewLine)
                {
                    text += Environment.NewLine;
                    lastIsNewLine = false;
                }
                if (joining)
                {
                    text += ' ';
                }
                else
                {
                    joining = true;
                }
                text += line.Data;
            }
        }

        return text.TrimEnd(Environment.NewLine.ToCharArray());
    }

    private static bool IsTagStart(string line)
    {
        return Regex.IsMatch(line, @"^@[^{]");
    }
}
