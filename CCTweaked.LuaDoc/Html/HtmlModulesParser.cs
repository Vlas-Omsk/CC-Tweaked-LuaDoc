using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

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

        var contentNode = _document.DocumentNode.SelectNodes("//*[@id='content']").Single();

        _enumerator = contentNode.ChildNodes.AsEnumerable().GetEnumerator();
        _enumerator.MoveToNextTaggedNode();

        yield return ParseBaseModule();

        if (_enumerator.MoveToNextTaggedNode())
        {
            if (_enumerator.Current.Name != "h3" || _enumerator.Current.InnerText != "Types")
                throw new Exception();

            while (_enumerator.MoveToNextTaggedNode())
                yield return ParseTypeModule();
        }
    }

    private Module ParseBaseModule()
    {
        if (_enumerator.Current.Name != "h1")
            throw new Exception();

        return ParseModule(_enumerator.Current.InnerText);
    }

    private Module ParseTypeModule()
    {
        if (_enumerator.Current.Name != "h3")
            throw new Exception();

        return ParseModule(_enumerator.Current.SelectNodes("span").Single().InnerText);
    }

    private Module ParseModule(string name)
    {
        var module = new Module()
        {
            Name = name
        };

        if (!_enumerator.MoveToNextTaggedNode())
            throw new Exception();

        if (_enumerator.Current.Name != "p")
            throw new Exception();

        module.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        foreach (var section in new HtmlSectionsParser(_enumerator).ParseSections())
        {
            if (section.Type == SectionType.SeeCollection)
                module.See = ((IEnumerable<string>)section.Data).ToArray();
            else
                throw new Exception();
        }

        if (_enumerator.Current.Name == "table" && _enumerator.Current.GetClasses().Single() == "definition-list")
        {
            if (!_enumerator.MoveToNextTaggedNode())
                throw new Exception();
        }

        if (_enumerator.Current.Name != "dl" || _enumerator.Current.GetClasses().Single() != "definition")
            throw new Exception();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            module.Definitions = new HtmlDefinitionsParser(enumerator).ParseDefinitions().ToArray();
        }

        return module;
    }
}
