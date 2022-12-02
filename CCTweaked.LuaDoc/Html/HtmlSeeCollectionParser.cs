using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

internal sealed class HtmlSeeCollectionParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlSeeCollectionParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public IEnumerable<string> ParseSeeCollection()
    {
        do
        {
            if (_enumerator.Current.Name != "li")
                throw new Exception();

            using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
            {
                enumerator.MoveToNextTaggedNode();
                yield return new HtmlSeeParser(enumerator).ParseSee();
            }
        }
        while (_enumerator.MoveToNextTaggedNode());
    }
}
