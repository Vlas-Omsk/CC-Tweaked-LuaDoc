using CCTweaked.LuaDoc.Entities.Description;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlDescriptionParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;
    private readonly string _basePath;

    public HtmlDescriptionParser(IEnumerator<HtmlNode> enumerator, string basePath)
    {
        _enumerator = enumerator;
        _basePath = basePath;
    }

    private bool IsCurrentDescriptionNode => IsDescriptionNode(_enumerator.Current);

    public IEnumerable<IDescriptionNode> ParseDescription()
    {
        while (IsCurrentDescriptionNode)
        {
            switch (_enumerator.Current.Name)
            {
                case "div":
                    if (_enumerator.Current.HasClass("admonition"))
                    {
                        yield return ParseAdmonition();
                    }
                    break;
                case "table":
                    // TODO: Generate table
                    break;
                case "#text":
                    yield return new TextNode(TextNodeStyle.Normal, _enumerator.Current.InnerText);
                    break;
                case "code":
                    yield return new CodeNode(_enumerator.Current.InnerText);
                    break;
                case "a":
                    yield return ParseLink();
                    break;
                case "span":
                    yield return new TextNode(TextNodeStyle.Normal, _enumerator.Current.InnerText.Trim());
                    break;
                case "p":
                    using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
                    {
                        enumerator.MoveNext();
                        yield return new ParagraphNode(ParagraphNodeType.Default, new HtmlDescriptionParser(enumerator, _basePath).ParseDescription().ToArray());
                    }
                    break;
                case "pre":
                    using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
                    {
                        enumerator.MoveNext();
                        yield return new ParagraphNode(ParagraphNodeType.Formatted, new HtmlDescriptionParser(enumerator, _basePath).ParseDescription().ToArray());
                    }
                    break;
                case "h2":
                    yield return new TextNode(TextNodeStyle.Header, _enumerator.Current.InnerText.Trim());
                    break;
                case "strong":
                    yield return new TextNode(TextNodeStyle.Bold, _enumerator.Current.InnerText.Trim());
                    break;
                case "em":
                    yield return new TextNode(TextNodeStyle.Italic, _enumerator.Current.InnerText.Trim());
                    break;
                case "ul":
                case "ol":
                    yield return ParseList();
                    break;
                default:
                    throw new UnexpectedHtmlElementException();
            }

            if (!_enumerator.MoveNext())
                break;
        }
    }

    private LinkNode ParseLink()
    {
        var href = _enumerator.Current.GetAttributeValue<string>("href", null);

        if (href == null)
            throw new Exception("Unexpected null href");

        var name = _enumerator.Current.InnerText;

        return HtmlLinkParser.ParseLink(_basePath, href, name);
    }

    private ListNode ParseList()
    {
        var items = new List<ListItemNode>();

        foreach (var node in _enumerator.Current.ChildNodes)
        {
            if (node.Name == "#text")
                continue;

            if (node.Name != "li")
                throw new UnexpectedHtmlElementException();

            using (var enumerator = node.ChildNodes.AsEnumerable().GetEnumerator())
            {
                enumerator.MoveNext();
                items.Add(new ListItemNode(new HtmlDescriptionParser(enumerator, _basePath).ParseDescription().ToArray()));
            }
        }

        return new ListNode(items.ToArray());
    }

    private AdmonitionNode ParseAdmonition()
    {
        var admonitionTypeClass = _enumerator.Current.GetClasses().Skip(1).Single();
        var delimiterIndex = admonitionTypeClass.IndexOf('-');
        var admonitionType = admonitionTypeClass[(delimiterIndex + 1)..];

        AdmonitionNodeType nodeType;

        switch (admonitionType)
        {
            case "tip":
                nodeType = AdmonitionNodeType.Tip;
                break;
            case "note":
                nodeType = AdmonitionNodeType.Note;
                break;
            case "caution":
                nodeType = AdmonitionNodeType.Caution;
                break;
            case "info":
                nodeType = AdmonitionNodeType.Info;
                break;
            default:
                throw new InvalidDataException();
        }

        using var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator();

        if (!enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();
        if (enumerator.Current.GetClasses().Single() != "admonition-heading")
            throw new UnexpectedHtmlElementException();

        if (!enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();

        return new AdmonitionNode(nodeType, new HtmlDescriptionParser(enumerator, _basePath).ParseDescription().ToArray());
    }

    public static bool IsDescriptionNode(HtmlNode node)
    {
        return
            (node.Name == "div" && (node.HasClass("admonition") || node.HasClass("recipe-container"))) ||
            (node.Name == "table" && !node.GetClasses().Any()) ||
            node.Name == "#text" ||
            node.Name == "code" ||
            node.Name == "a" ||
            node.Name == "span" ||
            node.Name == "p" ||
            node.Name == "pre" ||
            node.Name == "h2" ||
            node.Name == "ul" ||
            node.Name == "ol" ||
            node.Name == "strong" ||
            node.Name == "em";
    }
}
