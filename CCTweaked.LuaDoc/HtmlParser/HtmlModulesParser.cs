using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

public sealed class HtmlModulesParser
{
    private readonly string _path;
    private readonly HtmlDocument _document = new HtmlDocument();
    private IEnumerator<HtmlNode> _enumerator;

    public HtmlModulesParser(string path)
    {
        _path = path;
    }

    public IEnumerable<Module> ParseModules()
    {
        _document.Load(_path);

        var contentNode = _document.DocumentNode
            .SelectNodes("//*[@id='content']")
            .Single();

        _enumerator = contentNode.ChildNodes.AsEnumerable().GetEnumerator();
        _enumerator.MoveToNextTaggedNode();

        yield return ParseBaseModule();

        if (_enumerator.MoveToNextTaggedNode())
        {
            if (_enumerator.Current.Name != "h3" || _enumerator.Current.InnerText != "Types")
                throw new UnexpectedHtmlElementException();

            while (_enumerator.MoveToNextTaggedNode())
                yield return ParseTypeModule();
        }
    }

    private Module ParseBaseModule()
    {
        if (_enumerator.Current.Name != "h1")
            throw new UnexpectedHtmlElementException();

        return ParseModule(_enumerator.Current.InnerText, ModuleType.Module);
    }

    private Module ParseTypeModule()
    {
        if (_enumerator.Current.Name != "h3")
            throw new UnexpectedHtmlElementException();

        return ParseModule(
            _enumerator.Current
                .SelectNodes("span")
                .Single()
                .InnerText,
            ModuleType.Type
        );
    }

    private Module ParseModule(string name, ModuleType type)
    {
        var module = new Module(name, type);

        if (!_enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();

        module.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        foreach (var section in new HtmlSectionsParser(_enumerator).ParseSections())
        {
            if (section.Type == HtmlSectionType.SeeCollection)
                module.See = ((IEnumerable<See>)section.Data).ToArray();
            else
                throw new Exception();
        }

        if (
            _enumerator.Current.Name == "table" &&
            _enumerator.Current.GetClasses().Single() == "definition-list"
        )
        {
            if (!_enumerator.MoveToNextTaggedNode())
                throw new UnexpectedEndOfHtmlElementContentException();
        }

        if (
            _enumerator.Current.Name != "dl" ||
            _enumerator.Current.GetClasses().Single() != "definition"
        )
            throw new UnexpectedHtmlElementException();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            module.Definitions = new HtmlDefinitionsParser(enumerator)
                .ParseDefinitions(name)
                .ToArray();
        }

        return module;
    }
}
