using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

internal sealed class HtmlParametersParser : HtmlListParser<Parameter>
{
    public HtmlParametersParser(IEnumerator<HtmlNode> enumerator) : base(enumerator)
    {
    }

    protected override Parameter ParseItem(IEnumerator<HtmlNode> enumerator)
    {
        return new HtmlParameterParser(enumerator).ParseParameter();
    }
}
