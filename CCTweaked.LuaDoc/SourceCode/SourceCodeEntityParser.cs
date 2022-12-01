using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;
using CCTweaked.LuaDoc.SourceCode.Entities;

namespace CCTweaked.LuaDoc.SourceCode;

public sealed class SourceCodeEntityParser
{
    public SourceCodeEntityParser()
    {
    }

    public IEnumerable<Entity> Parse(IEnumerable<Block> blocks)
    {
        foreach (var block in blocks)
            yield return ParseEntity(block);
    }

    private Entity ParseEntity(Block block)
    {
        if (block.Tags.Any(x => x.Key == "module"))
            return ParseModuleEntity(block);
        else if (block.Data.Any(x => x.Contains("function")))
            return ParseFunctionEntity(block);
        else
            return ParseOtherEntity(block);
    }

    private Other ParseOtherEntity(Block block)
    {
        var other = new Other()
        {
            Description = NormalizeText(block.Description),
            Data = block.Data
        };

        var tagDictionary = new Dictionary<string, Tag[]>();

        foreach (var tags in block.Tags)
        {
            tagDictionary.Add(tags.Key, tags.Value);
        }

        other.Tags = tagDictionary.ToArray();

        return other;
    }

    private Module ParseModuleEntity(Block block)
    {
        var module = new Module()
        {
            Description = NormalizeText(block.Description),
        };

        var tagDictionary = new Dictionary<string, Tag[]>();

        foreach (var tags in block.Tags)
        {
            if (tags.Key == "module")
                module.Name = tags.Value.Single().Data;
            else
                tagDictionary.Add(tags.Key, tags.Value);
        }

        module.Tags = tagDictionary.ToArray();

        return module;
    }

    private Function ParseFunctionEntity(Block block)
    {
        var function = new Function()
        {
            Description = NormalizeText(block.Description),
            Name = Regex.Match(block.Data[0], @"function\s(.*?)\(").Groups[1].Value,
        };

        var overloadList = new List<Overload>();

        for (var i = 1; i < 999; i++)
        {
            var overload = ParseOverload(block, i);

            if (overload == null)
                break;

            overloadList.Add(overload);
        }

        function.Overloads = overloadList.ToArray();

        var tagDictionary = new Dictionary<string, Tag[]>();

        foreach (var tags in block.Tags)
        {
            if (tags.Key == "tparam" || tags.Key == "treturn")
                continue;

            if (tags.Key == "param")
                continue;

            tagDictionary.Add(tags.Key, tags.Value);
        }

        function.Tags = tagDictionary.ToArray();

        return function;
    }

    private int ParseTParamId(Tag tag)
    {
        var first = tag.Params.FirstOrDefault();

        if (int.TryParse(first.Key, out var result))
            return result;

        return 1;
    }

    private Overload ParseOverload(Block block, int index)
    {
        var @params = new List<Parameter>();
        var returns = new List<Return>();

        foreach (var tags in block.Tags)
        {
            if (tags.Key == "tparam")
            {
                foreach (var value in tags.Value)
                {
                    if (index != ParseTParamId(value))
                        continue;

                    var typeEnd = TypeParser.GetTypeEnd(value.Data, 0);
                    var nameEnd = TypeParser.GetWordEnd(value.Data, typeEnd + 1);

                    string type, name, description;

                    type = NormalizeType(value.Data[..typeEnd]);

                    if (nameEnd == -1)
                    {
                        name = value.Data[(typeEnd + 1)..];
                        description = null;
                    }
                    else
                    {
                        name = value.Data[(typeEnd + 1)..nameEnd];
                        description = value.Data[(nameEnd + 1)..];
                    }

                    @params.Add(new Parameter()
                    {
                        Name = name,
                        Type = type,
                        Optional = value.Params.Any(x => x.Key == "opt" && x.Value != "false"),
                        Description = NormalizeText(description)
                    });
                }
            }
            if (tags.Key == "treturn")
            {
                foreach (var value in tags.Value)
                {
                    if (index != ParseTParamId(value))
                        continue;

                    var typeEnd = TypeParser.GetTypeEnd(value.Data, 0);

                    string type, description;

                    type = NormalizeType(value.Data[..typeEnd]);
                    description = value.Data[(typeEnd + 1)..];

                    returns.Add(new Return()
                    {
                        Type = type,
                        Description = NormalizeText(description)
                    });
                }
            }
            if (index == 1 && tags.Key == "param")
            {
                foreach (var value in tags.Value)
                {
                    var match = Regex.Match(value.Data, @"([a-zA-Z.]+)\s*(.*)");

                    @params.Add(new Parameter()
                    {
                        Name = match.Groups[1].Value,
                        Type = "any",
                        Optional = false,
                        Description = NormalizeText(match.Groups[2].Value)
                    });
                }
            }
        }

        if (@params.Count == 0 && returns.Count == 0)
            return null;

        return new Overload()
        {
            Params = @params.ToArray(),
            Returns = returns.ToArray()
        };
    }

    private static string NormalizeType(string type)
    {
        type = type.Replace("function", "fun");
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{x.Groups[1].Value}:");
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{{ [number]: {x.Groups[1].Value} }}");

        return type;
    }

    private static string NormalizeText(string text)
    {
        if (text == null)
            return null;

        return Regex.Replace(text, "@{(.*?)}", x => $"`{x.Groups[1].Value}`");
    }
}
