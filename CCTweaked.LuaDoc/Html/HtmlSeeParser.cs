using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlSeeParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlSeeParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public string ParseSee()
    {
        if (_enumerator.Current.Name != "strong")
            throw new Exception();

        var text = _enumerator.Current.InnerText;

        if (_enumerator.MoveNext())
            text += " " + new HtmlDescriptionParser(_enumerator).ParseDescription();

        return text;
    }
}
