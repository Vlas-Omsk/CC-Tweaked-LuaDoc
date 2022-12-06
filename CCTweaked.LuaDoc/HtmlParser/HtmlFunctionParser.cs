using CCTweaked.LuaDoc.Entities;
using HtmlAgilityPack;

namespace CCTweaked.LuaDoc.HtmlParser;

internal sealed class HtmlFunctionParser
{
    private readonly IEnumerator<HtmlNode> _enumerator;

    public HtmlFunctionParser(IEnumerator<HtmlNode> enumerator)
    {
        _enumerator = enumerator;
    }

    public Function ParseFunction(string name, bool needSelf, string source)
    {
        var function = new Function(name, needSelf)
        {
            Source = source
        };

        if (_enumerator.Current != null)
            function.Description = new HtmlDescriptionParser(_enumerator).ParseDescription();

        if (_enumerator.Current != null)
        {
            var parametersOverloads = new List<FunctionOverload<Parameter>>();
            var returnsOverloads = new List<FunctionOverload<Return>>();

            foreach (var section in new HtmlSectionsParser(_enumerator).ParseSections())
            {
                switch (section.Type)
                {
                    case HtmlSectionType.Parameters:
                        {
                            var items = ((IEnumerable<Parameter>)section.Data).ToArray();

                            if (items.Length > 0)
                                parametersOverloads.Add(new FunctionOverload<Parameter>(items));
                            break;
                        }
                    case HtmlSectionType.Returns:
                        {
                            var items = ((IEnumerable<Return>)section.Data).ToArray();

                            if (items.Length > 0)
                                returnsOverloads.Add(new FunctionOverload<Return>(items));
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
            function.ParametersOverloads = Array.Empty<FunctionOverload<Parameter>>();
            function.ReturnsOverloads = Array.Empty<FunctionOverload<Return>>();
        }

        return function;
    }
}
