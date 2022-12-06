using System.Web;
using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlParameterParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlParameterParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Parameter ParseParameter()
    {
        if (
            _enumerator.Current.Name != "span" ||
            _enumerator.Current.GetClasses().Single() != "parameter"
        )
            throw new UnexpectedHtmlElementException();

        var parameter = new Parameter(_enumerator.Current.FirstChild.InnerText)
        {
            Optional = _enumerator.Current
                .SelectNodes("*[@class='optional']")?
                .Single() != null
        };

        while (_enumerator.MoveNext())
        {
            if (
                _enumerator.Current.Name == "span" ||
                !string.IsNullOrWhiteSpace(_enumerator.Current.InnerText)
            )
                break;
        }

        if (
            _enumerator.Current != null &&
            _enumerator.Current.Name == "span" &&
            _enumerator.Current.GetClasses().Single() == "type"
        )
        {
            parameter.Type = HttpUtility.HtmlDecode(_enumerator.Current.InnerText);
            _enumerator.MoveNext();
        }

        if (
            _enumerator.Current != null &&
            _enumerator.Current.Name == "span" &&
            _enumerator.Current.GetClasses().Single() == "default-value"
        )
        {
            parameter.DefaultValue = HttpUtility.HtmlDecode(_enumerator.Current.ChildNodes[1].InnerText);
            _enumerator.MoveNext();
        }

        if (_enumerator.Current != null)
            parameter.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        return parameter;
    }
}
