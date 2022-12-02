using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

internal sealed class HtmlSectionsParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlSectionsParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public IEnumerable<HtmlSection> ParseSections()
    {
        string section = null;

        do
        {
            if (_enumerator.Current.InnerText != "Or")
                section = _enumerator.Current.InnerText;

            if (section == null)
                throw new Exception();

            if (section == "Parameters")
            {
                yield return new HtmlSection(HtmlSectionType.Parameters, ParseParameters());
            }
            else if (section == "Returns")
            {
                yield return new HtmlSection(HtmlSectionType.Returns, ParseReturns());
            }
            else if (section == "See also")
            {
                yield return new HtmlSection(HtmlSectionType.SeeCollection, ParseSee());
            }
            else if (section == "Changes" || section == "Usage" || section == "Throws")
            {
                if (!_enumerator.MoveToNextTaggedNode())
                    throw new Exception();
            }
            else
            {
                break;
            }
        }
        while (_enumerator.MoveToNextTaggedNode());
    }

    private IEnumerable<Parameter> ParseParameters()
    {
        if (!_enumerator.MoveToNextTaggedNode())
            throw new Exception();

        if (_enumerator.Current.Name != "ol" || _enumerator.Current.GetClasses().Single() != "parameter-list")
            throw new Exception();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            return new HtmlParametersParser(enumerator).ParseList();
        }
    }

    private IEnumerable<Return> ParseReturns()
    {
        if (!_enumerator.MoveToNextTaggedNode())
            throw new Exception();

        if (_enumerator.Current.Name != "ol" || _enumerator.Current.GetClasses().Single() != "return-list")
            throw new Exception();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            return new HtmlReturnsParser(enumerator).ParseList();
        }
    }

    private IEnumerable<string> ParseSee()
    {
        if (!_enumerator.MoveToNextTaggedNode())
            throw new Exception();

        if (_enumerator.Current.Name != "ul")
            throw new Exception();

        using (var enumerator = _enumerator.Current.ChildNodes.AsEnumerable().GetEnumerator())
        {
            enumerator.MoveToNextTaggedNode();
            return new HtmlSeeCollectionParser(enumerator).ParseSeeCollection();
        }
    }
}
