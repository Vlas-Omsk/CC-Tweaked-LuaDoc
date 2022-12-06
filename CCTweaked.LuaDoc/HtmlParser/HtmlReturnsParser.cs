using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlReturnsParser : HtmlListParser<Return>
{
    public HtmlReturnsParser(IEnumerator<HtmlNode> enumerator) : base(enumerator)
    {
    }

    protected override Return ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        enumerator.MoveNext();
        return new HtmlReturnParser(enumerator).ParseReturn();
    }
}
