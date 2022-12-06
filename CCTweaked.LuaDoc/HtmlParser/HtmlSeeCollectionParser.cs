using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlSeeCollectionParser : HtmlListParser<See>
{
    public HtmlSeeCollectionParser(IEnumerator<HtmlNode> enumerator) : base(enumerator)
    {
    }

    protected override See ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        enumerator.MoveToNextTaggedNode();
        return new HtmlSeeParser(enumerator).ParseSee();
    }
}
