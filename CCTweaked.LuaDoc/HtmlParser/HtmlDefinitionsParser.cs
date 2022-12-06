using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlDefinitionsParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlDefinitionsParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public IEnumerable<Definition> ParseDefinitions(string moduleName)
    {
        do
        {
            yield return ParseDefinition(moduleName);
        }
        while (_enumerator.MoveToNextTaggedNode());
    }

    private Definition ParseDefinition(string moduleName)
    {
        if (_enumerator.Current.Name != "dt")
            throw new UnexpectedHtmlElementException();

        var definitionName = _enumerator.Current
            .SelectNodes("*[contains(concat(' ', @class, ' '), ' definition-name ')]")
            .First()
            .InnerText;

        if (definitionName.StartsWith(moduleName + '.'))
            definitionName = definitionName[(moduleName.Length + 1)..];

        bool needSelf = false;

        if (definitionName.StartsWith(moduleName + ':'))
        {
            definitionName = definitionName[(moduleName.Length + 1)..];
            needSelf = true;
        }

        var source = _enumerator.Current
            .SelectNodes("*[@class='source-link']")
            .SingleOrDefault()?
            .GetAttributeValue("href", null);

        if (!_enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();

        if (_enumerator.Current.Name != "dd")
            throw new UnexpectedHtmlElementException();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();

            var match = Regex.Match(definitionName, @"^([a-zA-Z_0-9]+)\s*=\s*(.+)");

            if (match.Success)
            {
                return new HtmlVariableParser(enumerator).ParseVariable(match.Groups[1].Value, match.Groups[2].Value, source);
            }
            else
            {
                match = Regex.Match(definitionName, @"^([a-zA-Z_0-9]+)\(");

                if (match.Success)
                    return new HtmlFunctionParser(enumerator).ParseFunction(match.Groups[1].Value, needSelf, source);
                else
                    return new HtmlVariableParser(enumerator).ParseVariable(definitionName, null, source);
            }
        }
    }
}
