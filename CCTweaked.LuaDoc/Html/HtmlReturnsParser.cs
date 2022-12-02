using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlReturnsParser : HtmlListParser<Return>
{
    public HtmlReturnsParser(IEnumerator<HtmlNode> enumerator) : base(enumerator)
    {
    }

    protected override Return ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        return new HtmlReturnParser(enumerator).ParseReturn();
    }
}
