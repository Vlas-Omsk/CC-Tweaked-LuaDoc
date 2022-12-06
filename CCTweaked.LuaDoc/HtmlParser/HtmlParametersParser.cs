using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlParametersParser : HtmlListParser<Parameter>
{
    public HtmlParametersParser(IEnumerator<HtmlNode> enumerator) : base(enumerator)
    {
    }

    protected override Parameter ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        enumerator.MoveToNextTaggedNode();
        return new HtmlParameterParser(enumerator).ParseParameter();
    }
}
