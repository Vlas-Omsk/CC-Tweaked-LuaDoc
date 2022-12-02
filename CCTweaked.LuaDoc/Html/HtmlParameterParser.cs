using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

internal sealed class HtmlParameterParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlParameterParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Parameter ParseParameter()
    {
        if (_enumerator.Current.Name != "span" || _enumerator.Current.GetClasses().Single() != "parameter")
            throw new Exception();

        var parameter = new Parameter()
        {
            Name = _enumerator.Current.FirstChild.InnerText,
            Optional = _enumerator.Current.SelectNodes("*[@class='optional']")?.Single() != null
        };

        while (_enumerator.MoveNext())
        {
            if (_enumerator.Current.Name == "span" || !string.IsNullOrWhiteSpace(_enumerator.Current.InnerText))
                break;
        }

        if (_enumerator.Current != null && _enumerator.Current.Name == "span" && _enumerator.Current.GetClasses().Single() == "type")
        {
            parameter.Type = TypeUtils.NormalizeType(_enumerator.Current.InnerText);

            _enumerator.MoveNext();
        }

        if (_enumerator.Current != null)
            parameter.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        return parameter;
    }
}
