using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlVariableParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;
    private readonly string _basePath;

    public HtmlVariableParser(IEnumerator<HtmlNode> enumerator, string basePath)
    {
        _enumerator = enumerator;
        _basePath = basePath;
    }

    public Variable ParseVariable(string name, string value, string source)
    {
        var variable = new Variable(name)
        {
            Value = value,
            Source = source
        };

        if (_enumerator.Current != null)
            variable.Description = new HtmlDescriptionParser(_enumerator, _basePath).ParseDescription().ToArray();

        return variable;
    }
}
