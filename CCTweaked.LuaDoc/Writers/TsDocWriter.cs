using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;
using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Writers;

public sealed class TsDocWriter : IDocWriter
{
    private static readonly string[] _reservedWords =
    {
        "delete",
        "default",
        "new"
    };
    private static readonly (string, string)[] _ignoreList =
    {
        ("io", "stdin"),
        ("io", "stdout"),
        ("io", "stderr")
    };
    private const string _globalObjectName = "_G";

    private readonly TsWriter _writer;

    private enum Scope
    {
        Global,
        Local,
        Member
    }

    public TsDocWriter(TsWriter writer)
    {
        _writer = writer;
    }

    public void Write(IEnumerable<Module> modules)
    {
        using var enumerator = modules.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new Exception();

        if (enumerator.Current.Type != ModuleType.Module)
            throw new Exception("Module is not a module type.");

        _writer.EnterComment();

        WriteDescription(enumerator.Current.Description);
        WriteSource(enumerator.Current.Source);
        WriteSeeCollection(enumerator.Current.See);

        _writer.ExitComment();

        var scope = enumerator.Current.Name == _globalObjectName ?
            Scope.Global :
            Scope.Local;

        if (scope == Scope.Local)
        {
            _writer.WriteLine($"declare namespace {enumerator.Current.Name} {{");

            _writer.IncreaseIndent();
        }
        else
        {
            _writer.WriteLine(null);
        }

        WriteDefinitions(
            enumerator.Current.Name,
            enumerator.Current.Definitions,
            scope
        );

        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Type != ModuleType.Type)
                throw new Exception("Module is not a type type.");

            _writer.EnterComment();

            WriteDescription(enumerator.Current.Description);
            WriteSource(enumerator.Current.Source);
            WriteSeeCollection(enumerator.Current.See);

            _writer.ExitComment();

            if (scope == Scope.Local)
                _writer.Write("export ");
            else
                _writer.Write("declare ");

            _writer.WriteLine($"interface {enumerator.Current.Name} {{");

            _writer.IncreaseIndent();

            WriteDefinitions(
                enumerator.Current.Name,
                enumerator.Current.Definitions,
                Scope.Member
            );

            _writer.DecreaseIndent();

            _writer.WriteLine("}");
            _writer.WriteLine(null);
        }

        if (scope == Scope.Local)
        {
            _writer.DecreaseIndent();

            _writer.WriteLine("}");
            _writer.WriteLine(null);
        }
    }

    private void WriteDefinitions(string moduleName, Definition[] definitions, Scope scope)
    {
        foreach (var definition in definitions)
        {
            if (IsIgnoreListContains(moduleName, definition.Name))
                continue;

            if (definition is Function function)
            {
                WriteFunction(function, scope);
            }
            else if (definition is Variable variable)
            {
                WriteVariable(variable, scope);
            }
            else
            {
                throw new ConversionNotSupportedForTypeException(definition.GetType());
            }
        }
    }

    private void WriteFunction(Function function, Scope scope)
    {
        foreach (var overload in function.CombineOverloadsWithMergedReturns())
        {
            WriteOverload(function, overload, scope);
        }
    }

    private void WriteOverload(Function function, OverloadWithMergedReturns overload, Scope scope)
    {
        _writer.EnterComment();

        WriteDescription(function.Description);
        WriteSource(function.Source);
        WriteSeeCollection(function.See);

        foreach (var parameter in overload.Parameters)
        {
            var parameterName = parameter.Name;

            if (parameterName == "...")
                parameterName = "params";
            else if (_reservedWords.Contains(parameterName))
                parameterName = $"_{parameterName}";

            _writer.Write($"@param {parameterName}");

            if (!string.IsNullOrWhiteSpace(parameter.DefaultValue))
                _writer.Write($" Default: `{parameter.DefaultValue}`.");

            if (parameter.Description != null && parameter.Description.Any())
            {
                _writer.Write(" ");
                new TsDescriptionWriter(_writer).WriteDescription(parameter.Description);
            }

            _writer.WriteLine(null);
        }

        if (overload.Returns.Length > 0)
        {
            for (var i = 0; i < overload.Returns[0].Length; i++)
            {
                _writer.Write($"@return ");

                for (var j = 0; j < overload.Returns.Length; j++)
                {
                    var description = overload.Returns[j][i].Description;

                    if (description != null && description.Any())
                    {
                        new TsDescriptionWriter(_writer).WriteDescription(description);
                        _writer.Write(" ");
                    }
                    else
                    {
                        _writer.Write("`null` ");
                    }

                    if (j != overload.Returns.Length - 1)
                        _writer.Write("**or** ");
                }

                _writer.WriteLine(null);
            }
        }

        if (!function.NeedSelf)
            _writer.WriteLine("@noSelf");

        _writer.ExitComment();

        var functionName = function.Name;
        var isOriginalReservedName = false;

        switch (scope)
        {
            case Scope.Local:
                if (_reservedWords.Contains(functionName))
                {
                    functionName = "_" + functionName;
                    isOriginalReservedName = true;
                }
                else
                {
                    _writer.Write("export ");
                }

                _writer.Write("function ");
                break;
            case Scope.Global:
                if (_reservedWords.Contains(functionName))
                    throw new Exception();

                _writer.Write("declare function ");
                break;
            case Scope.Member:
                break;
            default:
                throw new InvalidOperationException();
        }

        _writer.Write($"{functionName}(");

        var parameters = string.Join(", ", overload.Parameters.Select(x =>
        {
            var result = x.Name;
            var type = ConvertToTsType(x.Type);

            if (result == "...")
            {
                result += "params";
                type = $"({type})[]";
            }
            else if (_reservedWords.Contains(result))
            {
                result = $"_{result}";
            }

            if (x.Optional)
                result += "?";

            result += ": " + type;

            return result;
        }));

        _writer.Write(parameters + "): ");

        if (overload.Returns.Length == 0)
        {
            _writer.Write("void");
        }
        else if (overload.Returns.Length == 1)
        {
            var returns = overload.Returns[0];

            if (returns.Length == 1)
                _writer.Write(ConvertToTsType(returns[0].Type));
            else
                _writer.Write($"LuaMultiReturn<[{GetReturnsType(overload.Returns[0])}]>");
        }
        else
        {
            var returns =
                "LuaMultiReturn<[" +
                string.Join(
                    "]> | LuaMultiReturn<[",
                    overload.Returns
                        .Select(x => GetReturnsType(x))
                ) +
                "]>";

            _writer.Write(returns);
        }

        _writer.WriteLine(";");

        if (scope == Scope.Local && isOriginalReservedName)
            _writer.WriteLine($"export {{ {functionName} as {function.Name} }};");

        _writer.WriteLine(null);
    }

    private string GetReturnsType(Return[] returns)
    {
        return string.Join(
            ", ",
            returns.Select(x => ConvertToTsType(x.Type))
        );
    }

    private void WriteVariable(Variable variable, Scope scope)
    {
        _writer.EnterComment();

        WriteDescription(variable.Description);
        WriteSource(variable.Source);
        WriteSeeCollection(variable.See);

        _writer.ExitComment();

        switch (scope)
        {
            case Scope.Global:
                _writer.Write("declare ");
                break;
            case Scope.Local:
                _writer.Write("export ");
                break;
            case Scope.Member:
                break;
            default:
                throw new InvalidOperationException();
        }

        _writer.Write($"const {variable.Name}");

        if (!string.IsNullOrWhiteSpace(variable.Value))
            _writer.WriteLine($" = {variable.Value}");
        else
            _writer.WriteLine(": any");

        _writer.WriteLine(null);
    }

    private void WriteDescription(IDescriptionNode[] description)
    {
        if (description != null && description.Any())
        {
            new TsDescriptionWriter(_writer).WriteDescription(description);
            _writer.WriteLine(null);
            _writer.WriteLine(null);
        }
    }

    private void WriteSeeCollection(See[] seeCollection)
    {
        if (seeCollection != null)
        {
            foreach (var see in seeCollection)
            {
                _writer.Write($"@see");

                switch (see.Link.Type)
                {
                    case LinkNodeType.ExternalLink:
                    case LinkNodeType.TypeLink:
                        _writer.Write($" {{@link {see.Link.Link} {see.Link.Name}}}");
                        break;
                    default:
                        throw new InvalidDataException();
                }

                if (see.Description != null && see.Description.Any())
                {
                    _writer.Write(" ");
                    new TsDescriptionWriter(_writer).WriteDescription(see.Description);
                }

                _writer.WriteLine(null);
            }

            _writer.WriteLine(null);
        }
    }

    private void WriteSource(string source)
    {
        if (!string.IsNullOrWhiteSpace(source))
        {
            _writer.WriteLine($"[View source]({source})");
            _writer.WriteLine(null);
        }
    }

    private bool IsIgnoreListContains(string moduleName, string name)
    {
        if (_ignoreList.Any(x => x.Item1 == moduleName && x.Item2 == name))
            return true;

        return false;
    }

    private static string ConvertToTsType(string type)
    {
        var original = type;

        if (string.IsNullOrWhiteSpace(type))
            type = "any";

        type = type.Replace("nil", "null");
        type = type.Replace("table", "LuaTable");

        int index = 0;

        // Examples:
        // function
        // () => any
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/resources/data/computercraft/lua/rom/apis/parallel.lua#L120
        while (index < type.Length && (index = type.IndexOf("function", index)) != -1)
        {
            var endBracketIndex = FindEndBracket(type, index + "function".Length);

            if (endBracketIndex == -1)
            {
                type = type[..index] + "() => any" + type[(index + "function".Length)..];
            }

            index += "function".Length;
        }

        // Examples:
        // function(partial: string):{ string... } | nil
        // (partial: string) => string[] | null
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/doc/stub/global.lua#L111
        if (type.StartsWith("function"))
        {
            var endBracketIndex = FindEndBracket(type, "function".Length);
            var returnTypeDelimiterIndex = type.IndexOf(':', endBracketIndex);

            type = $"{(type["function".Length..(endBracketIndex + 1)])} => {ConvertToTsType(type[(returnTypeDelimiterIndex + 1)..])}";
        }

        // Examples:
        // { string }
        // (string)[]
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/resources/data/computercraft/lua/rom/apis/settings.lua#L173
        type = Regex.Replace(type, @"{\s*([a-zA-Z_]+?)\s*}", x => $"({ConvertToTsType(x.Groups[1].Value)})[]");

        // Examples:
        // { [string] = string }
        // { [key: string]: string }
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/doc/stub/http.lua#L80
        type = Regex.Replace(type, @"{\s*\[(.+?)\]\s*=\s*(.+?)\s*}", x => $"{{ [key: {ConvertToTsType(x.Groups[1].Value)}]: {ConvertToTsType(x.Groups[2].Value)} }}");

        // Examples:
        // { url = string, headers? = { [string] = string }, binary? = boolean, method? = string, redirect? = boolean }
        // { url: string, headers?: { [key: string]: string }, binary?: boolean, method?: string, redirect?: boolean }
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/doc/stub/http.lua#L80
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{ConvertToTsType(x.Groups[1].Value)}:");

        // Examples:
        // { string... }
        // string[]
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java#L104
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{ConvertToTsType(x.Groups[1].Value)}[]");

        // Examples:
        // string...
        // LuaMultiReturn<(string)[]>
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/resources/data/computercraft/lua/rom/apis/peripheral.lua#L155
        type = Regex.Replace(type, @"([a-zA-Z_]+?)\.\.\.", x => $"LuaMultiReturn<({ConvertToTsType(x.Groups[1].Value)})[]>");

        return type;
    }

    private static int FindEndBracket(string str, int startIndex)
    {
        for (; startIndex < str.Length; startIndex++)
        {
            var ch = str[startIndex];

            if (ch == '(')
                break;
            else if (!char.IsWhiteSpace(ch))
                return -1;
        }

        var depth = 0;

        for (; startIndex < str.Length; startIndex++)
        {
            var ch = str[startIndex];

            if (ch == '(')
            {
                depth++;
            }
            else if (ch == ')')
            {
                depth--;

                if (depth == 0)
                    return startIndex;
            }
        }

        return -1;
    }
}
