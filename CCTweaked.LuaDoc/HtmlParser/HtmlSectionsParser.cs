using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlSectionsParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;
    private readonly string _basePath;

    public HtmlSectionsParser(IEnumerator<HtmlNode> enumerator, string basePath)
    {
        _enumerator = enumerator;
        _basePath = basePath;
    }

    public IEnumerable<HtmlSection> ParseSections()
    {
        string section = null;

        do
        {
            if (_enumerator.Current.Name != "h3" && _enumerator.Current.InnerText != "Or")
                break;

            if (_enumerator.Current.InnerText != "Or")
                section = _enumerator.Current.InnerText;

            if (section == null)
                throw new UnexpectedHtmlElementException();

            switch (section)
            {
                case "Parameters":
                    yield return new HtmlSection(HtmlSectionType.Parameters, ParseParameters());
                    break;
                case "Returns":
                    yield return new HtmlSection(HtmlSectionType.Returns, ParseReturns());
                    break;
                case "See also":
                    yield return new HtmlSection(HtmlSectionType.SeeCollection, ParseSee());
                    break;
                case "Changes":
                case "Usage":
                case "Throws":
                    if (!_enumerator.MoveToNextTaggedNode())
                        throw new UnexpectedEndOfHtmlElementContentException();
                    break;
                default:
                    throw new Exception("Unexpected section name");
            }
        }
        while (_enumerator.MoveToNextTaggedNode());
    }

    private IEnumerable<Parameter> ParseParameters()
    {
        if (!_enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();

        if (
            _enumerator.Current.Name != "ol" ||
            _enumerator.Current.GetClasses().Single() != "parameter-list"
        )
            throw new UnexpectedHtmlElementException();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            return new HtmlParametersParser(enumerator, _basePath).ParseList();
        }
    }

    private IEnumerable<Return> ParseReturns()
    {
        if (!_enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();

        if (
            _enumerator.Current.Name != "ol" ||
            _enumerator.Current.GetClasses().Single() != "return-list"
        )
            throw new UnexpectedHtmlElementException();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            return new HtmlReturnsParser(enumerator, _basePath).ParseList();
        }
    }

    private IEnumerable<See> ParseSee()
    {
        if (!_enumerator.MoveToNextTaggedNode())
            throw new UnexpectedEndOfHtmlElementContentException();

        if (_enumerator.Current.Name != "ul")
            throw new UnexpectedHtmlElementException();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            return new HtmlSeeCollectionParser(enumerator, _basePath).ParseList();
        }
    }
}
