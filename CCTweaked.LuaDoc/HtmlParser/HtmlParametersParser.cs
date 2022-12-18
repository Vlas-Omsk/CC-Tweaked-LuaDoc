using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlParametersParser : HtmlListParser<Parameter>
{
    private readonly string _basePath;

    public HtmlParametersParser(IEnumerator<HtmlNode> enumerator, string basePath) : base(enumerator)
    {
        _basePath = basePath;
    }

    protected override Parameter ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        enumerator.MoveToNextTaggedNode();
        return new HtmlParameterParser(enumerator, _basePath).ParseParameter();
    }
}
