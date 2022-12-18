using System.Web;
using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlReturnParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;
    private readonly string _basePath;

    public HtmlReturnParser(IEnumerator<HtmlNode> enumerator, string basePath)
    {
        _enumerator = enumerator;
        _basePath = basePath;
    }

    public Return ParseReturn()
    {
        var @return = new Return();

        do
        {
            if (
                _enumerator.Current.Name == "span" ||
                !string.IsNullOrWhiteSpace(_enumerator.Current.InnerText)
            )
                break;
        }
        while (_enumerator.MoveNext());

        if (
            _enumerator.Current != null &&
            _enumerator.Current.Name == "span" &&
            _enumerator.Current.GetClasses().Single() == "type"
        )
        {
            @return.Type = HttpUtility.HtmlDecode(_enumerator.Current.InnerText);
            _enumerator.MoveNext();
        }

        if (_enumerator.Current != null)
            @return.Description = new HtmlDescriptionParser(_enumerator, _basePath).ParseDescription().ToArray();

        return @return;
    }
}
