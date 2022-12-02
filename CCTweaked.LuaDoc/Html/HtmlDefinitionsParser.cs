using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

internal sealed class HtmlDefinitionsParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlDefinitionsParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public IEnumerable<IDefinition> ParseDefinitions()
    {
        do
        {
            yield return ParseDefinition();
        }
        while (_enumerator.MoveToNextTaggedNode());
    }

    private IDefinition ParseDefinition()
    {
        if (_enumerator.Current.Name != "dt")
            throw new InvalidDataException();

        var definitionNameNode = _enumerator.Current.SelectNodes("*[contains(concat(' ', @class, ' '), ' definition-name ')]").First();

        if (!_enumerator.MoveToNextTaggedNode())
            throw new Exception();

        if (_enumerator.Current.Name != "dd")
            throw new InvalidDataException();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();

            var match = Regex.Match(definitionNameNode.InnerText, @"([a-zA-Z_0-9]+)\s*=\s*(.+)");

            if (match.Success)
            {
                return new HtmlVariableParser(enumerator).ParseVariable(match.Groups[1].Value, match.Groups[2].Value);
            }
            else
            {
                match = Regex.Match(definitionNameNode.InnerText, @"([a-zA-Z_0-9]+)\(");

                if (match.Success)
                    return new HtmlFunctionParser(enumerator).ParseFunction(match.Groups[1].Value);
                else
                    return new HtmlVariableParser(enumerator).ParseVariable(definitionNameNode.InnerText, null);
            }
        }
    }
}
