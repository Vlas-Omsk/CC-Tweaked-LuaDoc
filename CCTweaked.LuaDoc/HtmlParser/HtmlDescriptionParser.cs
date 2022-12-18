using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlDescriptionParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlDescriptionParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    private bool IsCurrentDescriptionNode => IsDescriptionNode(_enumerator.Current);

    public string ParseDescription()
    {
        var text = string.Empty;
        string prevNodeName = null;

        while (IsCurrentDescriptionNode)
        {
            string textToAdd = null;

            switch (_enumerator.Current.Name)
            {
                case "div":
                    textToAdd = ParseAdmonition();
                    break;
                case "table":
                    // TODO: Generate table
                    break;
                case "#text":
                    textToAdd = _enumerator.Current.InnerText.ReplaceLineEndings(" ").Trim();
                    break;
                case "code":
                    textToAdd = $"`{_enumerator.Current.InnerText.Trim()}`";
                    break;
                case "a":
                    textToAdd = ParseLink();
                    break;
                case "span":
                    textToAdd = _enumerator.Current.InnerText.Trim();
                    break;
                case "p":
                    using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
                    {
                        enumerator.MoveNext();
                        textToAdd = new HtmlDescriptionParser(enumerator).ParseDescription();
                    }
                    break;
                case "pre":
                    using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
                    {
                        enumerator.MoveNext();
                        textToAdd = $"```{Environment.NewLine}{new HtmlDescriptionParser(enumerator).ParseDescription()}{Environment.NewLine}```";
                    }
                    break;
                case "h2":
                case "strong":
                    textToAdd = $"**{_enumerator.Current.InnerText.Trim()}**";
                    break;
                case "em":
                    textToAdd = $"*{_enumerator.Current.InnerText.Trim()}*";
                    break;
                case "ul":
                case "ol":
                    textToAdd = ParseList();
                    break;
                default:
                    throw new UnexpectedHtmlElementException();
            }

            if (!string.IsNullOrWhiteSpace(textToAdd))
            {
                textToAdd = HttpUtility.HtmlDecode(textToAdd);

                if (
                    prevNodeName == "p" ||
                    prevNodeName == "pre" ||
                    prevNodeName == "div" ||
                    prevNodeName == "h2"
                )
                {
                    text += Environment.NewLine + Environment.NewLine + textToAdd;
                }
                else if (
                    prevNodeName == "#text" ||
                    prevNodeName == "code" ||
                    prevNodeName == "span" ||
                    prevNodeName == "strong" ||
                    prevNodeName == "em"
                )
                {
                    text += ' ' + textToAdd;
                }
                else
                {
                    text += textToAdd;
                }

                prevNodeName = _enumerator.Current.Name;
            }

            if (!_enumerator.MoveNext())
                break;
        }

        return text;
    }

    private string ParseLink()
    {
        var hrefAttribute = _enumerator.Current.GetAttributeValue<string>("href", null);

        if (hrefAttribute == null)
            throw new Exception("Unexpected null href");

        var match = Regex.Match(hrefAttribute, @"(.+)\.html(#v:(.+))?");

        var link = "{@link " + match.Groups[1].Value;

        if (match.Groups[2].Success)
            link += '.' + match.Groups[3].Value;

        return link + '}';
    }

    private string ParseList()
    {
        bool first = true;

        var result = "";

        foreach (var node in _enumerator.Current.ChildNodes)
        {
            if (node.Name == "#text")
                continue;

            if (first)
                first = false;
            else
                result += Environment.NewLine;

            if (node.Name != "li")
                throw new UnexpectedHtmlElementException();

            result += " - ";

            using (var enumerator = node.ChildNodes.AsEnumerable().GetEnumerator())
            {
                enumerator.MoveNext();
                result += new HtmlDescriptionParser(enumerator).ParseDescription();
            }
        }

        return result;
    }

    private string ParseAdmonition()
    {
        var admonitionTypeClass = _enumerator.Current.GetClasses().Skip(1).Single();
        var delimiterIndex = admonitionTypeClass.IndexOf('-');
        var admonitionType = admonitionTypeClass[(delimiterIndex + 1)..];
        var text = Environment.NewLine + Environment.NewLine;

        switch (admonitionType)
        {
            case "tip":
                text += "**Tip**";
                break;
            case "note":
                text += "**Note**";
                break;
            case "caution":
                text += "**Caution**";
                break;
            case "info":
                text += "**Info**";
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

        text += ": " + new HtmlDescriptionParser(enumerator).ParseDescription();

        return text;
    }

    public static bool IsDescriptionNode(HtmlNode node)
    {
        return
            (node.Name == "div" && node.HasClass("admonition")) ||
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
