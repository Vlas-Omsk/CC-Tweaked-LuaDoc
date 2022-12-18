using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlReturnsParser : HtmlListParser<Return>
{
    private readonly string _basePath;

    public HtmlReturnsParser(IEnumerator<HtmlNode> enumerator, string basePath) : base(enumerator)
    {
        _basePath = basePath;
    }

    protected override Return ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        enumerator.MoveNext();
        return new HtmlReturnParser(enumerator, _basePath).ParseReturn();
    }
}
