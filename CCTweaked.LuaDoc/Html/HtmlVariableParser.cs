using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlVariableParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlVariableParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Variable ParseVariable(string name, string value, string source)
    {
        var variable = new Variable()
        {
            Name = name,
            Value = value,
            Source = source
        };

        if (_enumerator.Current != null)
            variable.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        return variable;
    }
}
