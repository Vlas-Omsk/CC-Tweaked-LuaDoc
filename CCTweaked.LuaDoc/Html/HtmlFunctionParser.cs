using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.Html;

internal sealed class HtmlFunctionParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlFunctionParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Function ParseFunction(string name, bool isInstanceFunction, string source)
    {
        var function = new Function()
        {
            Name = name,
            IsInstance = isInstanceFunction,
            Source = source
        };

        if (_enumerator.Current != null)
            function.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        if (_enumerator.Current != null)
        {
            var parametersOverloads = new List<OverloadCollection<Parameter>>();
            var returnsOverloads = new List<OverloadCollection<Return>>();

            foreach (var section in new HtmlSectionsParser(_enumerator).ParseSections())
            {
                switch (section.Type)
                {
                    case HtmlSectionType.Parameters:
                        {
                            var items = ((IEnumerable<Parameter>)section.Data).ToArray();

                            if (items.Length > 0)
                                parametersOverloads.Add(new OverloadCollection<Parameter>()
                                {
                                    Items = items
                                });
                            break;
                        }
                    case HtmlSectionType.Returns:
                        {
                            var items = ((IEnumerable<Return>)section.Data).ToArray();

                            if (items.Length > 0)
                                returnsOverloads.Add(new OverloadCollection<Return>()
                                {
                                    Items = items
                                });
                            break;
                        }
                    case HtmlSectionType.SeeCollection:
                        function.See = ((IEnumerable<See>)section.Data).ToArray();
                        break;
                }
            }

            function.ParametersOverloads = parametersOverloads.ToArray();
            function.ReturnsOverloads = returnsOverloads.ToArray();
        }
        else
        {
            function.ParametersOverloads = Array.Empty<OverloadCollection<Parameter>>();
            function.ReturnsOverloads = Array.Empty<OverloadCollection<Return>>();
        }

        return function;
    }
}
