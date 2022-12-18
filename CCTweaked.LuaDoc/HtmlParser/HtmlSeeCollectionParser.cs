using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlSeeCollectionParser : HtmlListParser<See>
{
    private readonly string _basePath;

    public HtmlSeeCollectionParser(IEnumerator<HtmlNode> enumerator, string basePath) : base(enumerator)
    {
        _basePath = basePath;
    }

    protected override See ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        enumerator.MoveToNextTaggedNode();
        return new HtmlSeeParser(enumerator, _basePath).ParseSee();
    }
}
