using System.Web;
using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlReturnParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlReturnParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Return ParseReturn()
    {
        var @return = new Return();

        do
        {
            if (_enumerator.Current.Name == "span" || !string.IsNullOrWhiteSpace(_enumerator.Current.InnerText))
                break;
        }
        while (_enumerator.MoveNext());

        if (_enumerator.Current != null && _enumerator.Current.Name == "span" && _enumerator.Current.GetClasses().Single() == "type")
        {
            @return.Type = TypeUtils.NormalizeType(HttpUtility.HtmlDecode(_enumerator.Current.InnerText));
            _enumerator.MoveNext();
        }

        if (_enumerator.Current != null)
            @return.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        return @return;
    }
}
