using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

public sealed class HtmlFunctionParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlFunctionParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Function ParseFunction(string name, bool isInstanceFunction)
    {
        var function = new Function()
        {
            Name = name,
            IsInstance = isInstanceFunction
        };

        if (_enumerator.Current != null)
            function.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        if (_enumerator.Current != null)
        {
            var parametersOverloads = new List<Overload<Parameter>>();
            var returnsOverloads = new List<Overload<Return>>();

            foreach (var section in new HtmlSectionsParser(_enumerator).ParseSections())
            {
                switch (section.Type)
                {
                    case SectionType.Parameters:
                        {
                            var items = ((IEnumerable<Parameter>)section.Data).ToArray();

                            if (items.Length > 0)
                                parametersOverloads.Add(new Overload<Parameter>()
                                {
                                    Items = items
                                });
                            break;
                        }
                    case SectionType.Returns:
                        {
                            var items = ((IEnumerable<Return>)section.Data).ToArray();

                            if (items.Length > 0)
                                returnsOverloads.Add(new Overload<Return>()
                                {
                                    Items = items
                                });
                            break;
                        }
                    case SectionType.SeeCollection:
                        function.See = ((IEnumerable<string>)section.Data).ToArray();
                        break;
                }
            }

            function.ParametersOverloads = parametersOverloads.ToArray();
            function.ReturnsOverloads = returnsOverloads.ToArray();
        }
        else
        {
            function.ParametersOverloads = Array.Empty<Overload<Parameter>>();
            function.ReturnsOverloads = Array.Empty<Overload<Return>>();
        }

        return function;
    }
}
