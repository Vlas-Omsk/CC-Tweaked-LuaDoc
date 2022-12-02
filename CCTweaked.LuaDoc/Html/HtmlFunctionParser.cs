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

    public Function ParseFunction(string name)
    {
        var function = new Function()
        {
            Name = name,
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
                            var overloads = ((IEnumerable<Parameter>)section.Data).ToArray();

                            if (overloads.Length > 0)
                                parametersOverloads.Add(new Overload<Parameter>()
                                {
                                    Overloads = overloads
                                });
                            break;
                        }
                    case SectionType.Returns:
                        {
                            var overloads = ((IEnumerable<Return>)section.Data).ToArray();

                            if (overloads.Length > 0)
                                returnsOverloads.Add(new Overload<Return>()
                                {
                                    Overloads = overloads
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

        return function;
    }
}
