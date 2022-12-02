using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlParser
{
    private readonly string _path;
    private readonly HtmlDocument _document = new HtmlDocument();
    private IEnumerator<HtmlNode> _enumerator;

    public HtmlParser(string path)
    {
        _path = path;
    }

    public void Start()
    {
        _document.Load(_path);

        var contentNode = _document.DocumentNode.SelectNodes("//*[@id='content']").Single();

        _enumerator = contentNode.ChildNodes.AsEnumerable().GetEnumerator();

        while (_enumerator.MoveNext())
        {
            ParseSection();
        }
    }

    private void ParseSection()
    {
        if (_enumerator.Current.Name == "h1")
            ParseModule();
    }

    private void ParseModule()
    {
        var module = new Module()
        {
            Name = _enumerator.Current.InnerText
        };

        if (!_enumerator.MoveNext())
            throw new Exception();

        if (_enumerator.Current.Name != "p")
            throw new Exception();

        module.Description = ParseDescription(_enumerator.Current);
    }

    private void ParseFunctions(HtmlNode rootNode)
    {
        var definitionNode = rootNode.SelectNodes("//*[@class='definition']").Single();

        for (var i = 0; i < definitionNode.ChildNodes.Count; i += 2)
        {
            var dtNode = definitionNode.ChildNodes[i];
            if (dtNode.Name != "dt")
                throw new InvalidDataException();

            var ddNode = definitionNode.ChildNodes[i + 1];
            if (ddNode.Name != "dd")
                throw new InvalidDataException();

            ParseFunction(dtNode, ddNode);
        }
    }

    private string ParseDescription()
    {
        var text = string.Empty;

        while (_enumerator.Current.Name == "p")
        {
            foreach (var node in _enumerator.Current.ChildNodes)
            {

            }
        }

        return node.InnerText;
    }

    private void ParseFunction(HtmlNode dtNode, HtmlNode ddNode)
    {
        var definitionNameNode = dtNode.SelectNodes("//*[@class='definition-name']");


    }
}
