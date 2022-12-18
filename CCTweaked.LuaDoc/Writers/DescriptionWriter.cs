using System.Web;
using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Writers;

public abstract class DescriptionWriter
{
    private readonly IWriter _writer;
    private int _newLinesCount = 0;
    private bool _isCursorOnBegin = true;

    public DescriptionWriter(IWriter writer)
    {
        _writer = writer;
    }

    public bool WithoutLineEndings { get; set; }

    protected bool FormatLinkNodes { get; private set; } = true;

    public void WriteDescription(IDescriptionNode[] description)
    {
        foreach (var node in description)
        {
            if (node is AdmonitionNode admonitionNode)
            {
                WriteLine(null);
                WriteLine(null);
                Write($"**{admonitionNode.Type}**: ");
                _isCursorOnBegin = true;
                WriteDescription(admonitionNode.Description);
                _isCursorOnBegin = false;
            }
            else if (node is CodeNode codeNode)
            {
                FlushNewLines();

                Write($"`{codeNode.Content}`");
            }
            else if (node is LinkNode linkNode)
            {
                WriteLinkNode(linkNode);
            }
            else if (node is ListNode listNode)
            {
                WriteLine(null);

                for (var i = 0; i < listNode.Items.Length; i++)
                {
                    Write(" - ");
                    _isCursorOnBegin = true;
                    WriteDescription(listNode.Items[i].Description);
                    _isCursorOnBegin = false;

                    if (i != listNode.Items.Length - 1)
                        WriteLine(null);
                }
            }
            else if (node is ParagraphNode paragraphNode)
            {
                WriteLine(null);

                switch (paragraphNode.Type)
                {
                    case ParagraphNodeType.Default:
                        WriteLine(null);
                        _isCursorOnBegin = true;
                        WriteDescription(paragraphNode.Description);
                        _isCursorOnBegin = false;
                        break;
                    case ParagraphNodeType.Formatted:
                        WriteLine("```");
                        _isCursorOnBegin = true;
                        FormatLinkNodes = false;
                        WriteDescription(paragraphNode.Description);
                        _isCursorOnBegin = false;
                        FormatLinkNodes = true;
                        WriteLine(null);
                        Write("```");
                        _newLinesCount += 1;
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }
            else if (node is TextNode textNode)
            {
                switch (textNode.Style)
                {
                    case TextNodeStyle.Normal:
                        var content = TrimStartNewLines(textNode.Content);

                        if (string.IsNullOrEmpty(content))
                            continue;

                        FlushNewLines();

                        content = TrimEndNewLines(content);

                        Write(content);
                        break;
                    case TextNodeStyle.Bold:
                        Write($"**{textNode.Content}**");
                        break;
                    case TextNodeStyle.Italic:
                        Write($"***{textNode.Content}***");
                        break;
                    case TextNodeStyle.Header:
                        WriteLine(null);
                        WriteLine(null);
                        Write($"**{textNode.Content}**");
                        _newLinesCount += 2;
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }
        }
    }

    protected abstract void WriteLinkNode(LinkNode linkNode);

    protected void WriteLine(string str)
    {
        if (str == null)
        {
            if (_isCursorOnBegin || WithoutLineEndings)
                return;

            _newLinesCount = 0;
        }

        if (str != null && WithoutLineEndings)
            str = str.ReplaceLineEndings(" ");

        if (str == null && WithoutLineEndings)
            _writer.Write(" ");
        else
            _writer.WriteLine(HttpUtility.HtmlDecode(str));

        _isCursorOnBegin = false;
    }

    protected void Write(string str)
    {
        if (str == null && _isCursorOnBegin)
            return;

        if (str != null && WithoutLineEndings)
            str = str.ReplaceLineEndings(" ");

        _writer.Write(HttpUtility.HtmlDecode(str));

        _isCursorOnBegin = false;
    }

    private string TrimStartNewLines(string str)
    {
        if (str.Length == 0)
            return str;

        if (str.Length == 1)
        {
            if (str[0] == '\n')
            {
                _newLinesCount++;
                return string.Empty;
            }
            else
            {
                return str;
            }
        }

        var start = 0;

        for (; start < str.Length; start++)
        {
            if (str[start] != '\n')
                break;

            _newLinesCount++;
        }

        return str[start..str.Length];
    }

    private string TrimEndNewLines(string str)
    {
        if (str.Length == 0)
            return str;

        if (str.Length == 1)
        {
            if (str[0] == '\n')
            {
                _newLinesCount++;
                return string.Empty;
            }
            else
            {
                return str;
            }
        }

        var end = str.Length - 1;

        for (; end >= 0; end--)
        {
            if (str[end] != '\n')
                break;

            _newLinesCount++;
        }

        return str[0..(end + 1)];
    }

    protected void FlushNewLines()
    {
        if (!_isCursorOnBegin && !WithoutLineEndings)
        {
            for (var i = 0; i < _newLinesCount; i++)
            {
                if (WithoutLineEndings)
                    _writer.Write(" ");
                else
                    _writer.WriteLine(null);
            }
        }

        _newLinesCount = 0;
    }
}
