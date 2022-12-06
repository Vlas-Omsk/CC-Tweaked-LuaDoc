using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal abstract class HtmlListParser<T>
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlListParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public IEnumerable<T> ParseList()
    {
        do
        {
            if (_enumerator.Current.Name != "li")
                throw new UnexpectedHtmlElementException();

            using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
            {
                yield return ParseItem(enumerator);
            }
        }
        while (_enumerator.MoveToNextTaggedNode());
    }

    protected abstract T ParseItem(IEnumerator<HtmlNode> enumerator);
}
