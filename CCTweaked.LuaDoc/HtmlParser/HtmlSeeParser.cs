using CCTweaked.LuaDoc.Entities;
using CCTweaked.LuaDoc.Entities.Description;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlSeeParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;
    private readonly string _basePath;

    public HtmlSeeParser(IEnumerator<HtmlNode> enumerator, string basePath)
    {
        _enumerator = enumerator;
        _basePath = basePath;
    }

    public See ParseSee()
    {
        if (_enumerator.Current.Name != "strong")
            throw new UnexpectedHtmlElementException();

        var see = new See();

        if (_enumerator.Current.ChildNodes.Count != 1)
            throw new UnexpectedHtmlElementException();

        var href = _enumerator.Current.ChildNodes[0].GetAttributeValue("href", null);

        if (href == null)
            throw new Exception("Unexpected null href");

        var name = _enumerator.Current.ChildNodes[0].InnerText;

        var node = HtmlLinkParser.ParseLink(_basePath, href, name);

        if (_enumerator.MoveNext())
        {
            see.Link = node;
            see.Description = new HtmlDescriptionParser(_enumerator, _basePath).ParseDescription().ToArray();
        }
        else
        {
            see.Description = new[] { new TextNode(TextNodeStyle.Normal, name) };
        }

        return see;
    }
}
