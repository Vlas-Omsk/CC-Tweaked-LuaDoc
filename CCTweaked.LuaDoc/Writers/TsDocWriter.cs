using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc.Writers;

public sealed class TsDocWriter : IDocWriter, IDisposable
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
    private readonly TextWriter _writer;
    private int _indent;
    private bool _comment;
    private bool _isCursorOnNewLine = true;

    private enum Scope
    {
        Global,
        Local,
        Member
    }

    public TsDocWriter(string path) : this(new StreamWriter(path))
    {
    }

    public TsDocWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(IEnumerable<Module> modules)
    {
        using var enumerator = modules.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new Exception();

        if (enumerator.Current.IsType)
            throw new Exception();

        EnterComment();

        WriteDescription(enumerator.Current.Description);
        WriteSource(enumerator.Current.Source);
        WriteSeeCollection(enumerator.Current.See);

        ExitComment();

        var scope = enumerator.Current.Name == _globalObjectName ?
            Scope.Global :
            Scope.Local;

        if (scope == Scope.Local)
        {
            WriteLine($"declare namespace {enumerator.Current.Name} {{");

            IncreaseIndent();
        }
        else
        {
            WriteLine(null);
        }

        WriteDefinitions(enumerator.Current.Name, enumerator.Current.Definitions, scope);

        while (enumerator.MoveNext())
        {
            if (!enumerator.Current.IsType)
                throw new Exception();

            EnterComment();

            WriteDescription(enumerator.Current.Description);
            WriteSource(enumerator.Current.Source);
            WriteSeeCollection(enumerator.Current.See);

            ExitComment();

            if (scope == Scope.Local)
                Write("export ");
            else
                Write("declare ");

            WriteLine($"interface {enumerator.Current.Name} {{");

            IncreaseIndent();

            WriteDefinitions(enumerator.Current.Name, enumerator.Current.Definitions, Scope.Member);

            DecreaseIndent();

            WriteLine("}");
            WriteLine(null);
        }

        if (scope == Scope.Local)
        {
            DecreaseIndent();

            WriteLine("}");
            WriteLine(null);
        }
    }

    private void WriteDefinitions(string moduleName, IDefinition[] definitions, Scope scope)
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
                throw new Exception();
            }
        }
    }

    private void WriteFunction(Function function, Scope scope)
    {
        foreach (var overload in FunctionUtils.CombineOverloads(function))
        {
            WriteOverload(function, overload, scope);
        }
    }

    private void WriteOverload(Function function, Overload<Return[]> overload, Scope scope)
    {
        EnterComment();

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

            Write($"@param {parameterName}");

            if (!string.IsNullOrWhiteSpace(parameter.DefaultValue))
                Write($" Default: `{parameter.DefaultValue}`.");

            if (!string.IsNullOrWhiteSpace(parameter.Description))
                Write($" {parameter.Description}");

            WriteLine(null);
        }

        if (overload.Returns.Length > 0)
        {
            for (var i = 0; i < overload.Returns[0].Length; i++)
            {
                Write($"@return ");

                for (var j = 0; j < overload.Returns.Length; j++)
                {
                    var description = overload.Returns[j][i].Description;

                    if (string.IsNullOrWhiteSpace(description))
                        description = "`null`";

                    Write($"{description} ");

                    if (j != overload.Returns.Length - 1)
                        Write("**or** ");
                }

                WriteLine(null);
            }
        }

        if (!function.NeedSelf)
            WriteLine("@noSelf");

        ExitComment();

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
                    Write("export ");
                }

                Write("function ");
                break;
            case Scope.Global:
                if (_reservedWords.Contains(functionName))
                    throw new Exception();

                Write("declare function ");
                break;
            case Scope.Member:
                break;
            default:
                throw new InvalidDataException();
        }

        Write($"{functionName}(");

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

        Write(parameters + "): ");

        if (overload.Returns.Length == 0)
        {
            Write("void");
        }
        else if (overload.Returns.Length == 1)
        {
            var returns = overload.Returns[0];

            if (returns.Length == 1)
                Write(ConvertToTsType(returns[0].Type));
            else
                Write($"LuaMultiReturn<[{GetReturnsType(overload.Returns[0])}]>");
        }
        else
        {
            var returns = "LuaMultiReturn<[" + string.Join("]> | LuaMultiReturn<[", overload.Returns.Select(x => GetReturnsType(x))) + "]>";

            Write(returns);
        }

        WriteLine(";");

        if (scope == Scope.Local && isOriginalReservedName)
            WriteLine($"export {{ {functionName} as {function.Name} }};");

        WriteLine(null);
    }

    private string GetReturnsType(Return[] returns)
    {
        return string.Join(", ", returns.Select(x => ConvertToTsType(x.Type)));
    }

    private void WriteVariable(Variable variable, Scope scope)
    {
        EnterComment();

        WriteDescription(variable.Description);
        WriteSource(variable.Source);
        WriteSeeCollection(variable.See);

        ExitComment();

        switch (scope)
        {
            case Scope.Global:
                Write("declare ");
                break;
            case Scope.Local:
                Write("export ");
                break;
            case Scope.Member:
                break;
            default:
                throw new InvalidDataException();
        }

        Write($"const {variable.Name}");

        if (!string.IsNullOrWhiteSpace(variable.Value))
            WriteLine($" = {variable.Value}");
        else
            WriteLine(": any");

        WriteLine(null);
    }

    private void WriteDescription(string description)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            WriteLine(description);
            WriteLine(null);
        }
    }

    private void WriteSeeCollection(See[] seeCollection)
    {
        if (seeCollection != null)
        {
            foreach (var see in seeCollection)
            {
                Write($"@see");

                if (!string.IsNullOrWhiteSpace(see.Link))
                    Write($" {{@link {see.Link.Replace(':', '.')}}}");

                if (!string.IsNullOrWhiteSpace(see.Description))
                    Write($" {see.Description}");

                WriteLine(null);
            }

            WriteLine(null);
        }
    }

    private void WriteSource(string source)
    {
        if (!string.IsNullOrWhiteSpace(source))
        {
            WriteLine($"[View source]({source})");
            WriteLine(null);
        }
    }

    private void WriteLine(string str)
    {
        Write(str);
        _writer.WriteLine();
        _isCursorOnNewLine = true;
    }

    private void Write(string str)
    {
        if (str == null)
        {
            WriteInternal(str);
            return;
        }

        var lines = str.Split(Environment.NewLine);

        for (var i = 0; i < lines.Length - 1; i++)
        {
            WriteInternal(lines[i]);
            _writer.WriteLine();
            _isCursorOnNewLine = true;
        }

        WriteInternal(lines[lines.Length - 1]);
    }

    private void WriteInternal(string str)
    {
        if (_isCursorOnNewLine)
        {
            if (_indent > 0)
                _writer.Write(GetIndent());
            if (_comment)
                _writer.Write(" * ");

            _isCursorOnNewLine = false;
        }

        if (str != null)
        {
            if (_comment)
                str = str.Replace("*/", "*⁠/");

            _writer.Write(str);
        }
    }

    private string GetIndent()
    {
        return new string(' ', _indent * 2);
    }

    private void EnterComment()
    {
        _comment = true;
        _writer.WriteLine(GetIndent() + "/**");
    }

    private void ExitComment()
    {
        _comment = false;
        _writer.WriteLine(GetIndent() + " */");
    }

    private void IncreaseIndent()
    {
        _indent++;
    }

    private void DecreaseIndent()
    {
        if (_indent == 0)
            throw new Exception();

        _indent--;
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

        while (index < type.Length && (index = type.IndexOf("function", index)) != -1)
        {
            var endBracketIndex = FindEndBracket(type, index + "function".Length);

            if (endBracketIndex == -1)
            {
                type = type[..index] + "() => any" + type[(index + "function".Length)..];
            }

            index += "function".Length;
        }

        if (type.StartsWith("function"))
        {
            var endBracketIndex = FindEndBracket(type, "function".Length);
            var returnTypeDelimiterIndex = type.IndexOf(':', endBracketIndex);

            type = $"{(type["function".Length..(endBracketIndex + 1)])} => {ConvertToTsType(type[(returnTypeDelimiterIndex + 1)..])}";
        }

        type = Regex.Replace(type, @"{\s*([a-zA-Z_]+?)\s*}", x => $"({ConvertToTsType(x.Groups[1].Value)})[]");
        type = Regex.Replace(type, @"{\s*\[(.+?)\]\s*=\s*(.+?)\s*}", x => $"{{ [key: {ConvertToTsType(x.Groups[1].Value)}]: {ConvertToTsType(x.Groups[2].Value)} }}");
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{ConvertToTsType(x.Groups[1].Value)}:");
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{ConvertToTsType(x.Groups[1].Value)}[]");
        type = Regex.Replace(type, @"([a-zA-Z_]+?)\.\.\.", x => $"({ConvertToTsType(x.Groups[1].Value)})[]");

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

    public void Dispose()
    {
        _writer.Dispose();
    }
}
