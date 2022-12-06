using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlSeeParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlSeeParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public See ParseSee()
    {
        if (_enumerator.Current.Name != "strong")
            throw new UnexpectedHtmlElementException();

        var see = new See();
        var text = _enumerator.Current.InnerText;

        if (_enumerator.MoveNext())
        {
            see.Link = text;
            see.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();
        }
        else
        {
            see.Description = text;
        }

        return see;
    }
}
