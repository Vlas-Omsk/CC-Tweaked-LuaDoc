using System.Web;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

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
                    textToAdd = ParseAdmonition(_enumerator.Current);
                    break;
                case "table":
                    // TODO: Generate table
                    break;
                case "#text":
                    textToAdd = _enumerator.Current.InnerText.ReplaceLineEndings(" ");
                    break;
                case "code":
                    textToAdd = $"`{_enumerator.Current.InnerText.Trim()}`";
                    break;
                case "a":
                    textToAdd = $"{_enumerator.Current.InnerText.Trim()}";
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
                    textToAdd = $"**{_enumerator.Current.InnerText}**";
                    break;
                case "ul":
                    bool first = true;

                    textToAdd = string.Empty;

                    foreach (var node in _enumerator.Current.ChildNodes)
                    {
                        if (node.Name == "#text")
                            continue;

                        if (first)
                            first = false;
                        else
                            textToAdd += Environment.NewLine;

                        if (node.Name != "li")
                            throw new Exception();

                        textToAdd += " - ";

                        using (var enumerator = node.ChildNodes.AsEnumerable().GetEnumerator())
                        {
                            enumerator.MoveNext();
                            textToAdd += new HtmlDescriptionParser(enumerator).ParseDescription();
                        }
                    }
                    break;
                default:
                    throw new Exception();
            }

            if (!string.IsNullOrWhiteSpace(textToAdd))
            {
                if (prevNodeName == "p" || prevNodeName == "pre" || prevNodeName == "div")
                    text += Environment.NewLine + Environment.NewLine;

                prevNodeName = _enumerator.Current.Name;

                text += HttpUtility.HtmlDecode(textToAdd);
            }

            if (!_enumerator.MoveNext())
                break;
        }

        return text;
    }

    private string ParseAdmonition(HtmlNode node)
    {
        var admonitionTypeClass = node.GetClasses().Skip(1).Single();
        var delimiterIndex = admonitionTypeClass.IndexOf('-');
        var admonitionType = admonitionTypeClass[(delimiterIndex + 1)..];
        var text = string.Empty;

        switch (admonitionType)
        {
            case "tip":
                text += " - **Tip**";
                break;
            case "note":
                text += " - **Note**";
                break;
            case "caution":
                text += " - **Caution**";
                break;
            case "info":
                text += " - **Info**";
                break;
            default:
                throw new Exception();
        }

        using var enumerator = node.ChildNodes.AsEnumerable().GetEnumerator();

        if (!enumerator.MoveToNextTaggedNode())
            throw new Exception();
        if (enumerator.Current.GetClasses().Single() != "admonition-heading")
            throw new Exception();

        if (!enumerator.MoveToNextTaggedNode())
            throw new Exception();

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
            node.Name == "strong";
    }
}
