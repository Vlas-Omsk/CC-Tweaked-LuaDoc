using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc.Writers;

public sealed class LuaDocWriter : IDocWriter, IDisposable
{
    private readonly TextWriter _writer;
    private bool _comment;
    private bool _isCursorOnNewLine = true;

    public LuaDocWriter(string path) : this(new StreamWriter(path))
    {
    }

    public LuaDocWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(IEnumerable<Module> modules)
    {
        using var enumerator = modules.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new Exception();

        var baseModule = enumerator.Current;

        WriteBaseModule(baseModule);

        while (enumerator.MoveNext())
        {
            WriteTypeModule(baseModule, enumerator.Current);
        }
    }

    private void WriteBaseModule(Module module)
    {
        if (module.Type != ModuleType.Module)
            throw new Exception("Module is not a module type.");

        EnterComment();

        WriteLine("@meta");

        ExitComment();

        WriteLine(null);

        EnterComment();

        WriteDescription(module.Description);
        WriteSource(module.Source);
        WriteSeeCollection(module.See);

        WriteLine($"@class {module.Name}lib");

        ExitComment();

        WriteLine($"{module.Name} = {{}}");
        WriteLine(null);

        WriteDefinitions(module);
    }

    private void WriteTypeModule(Module baseModule, Module module)
    {
        if (module.Type != ModuleType.Type)
            throw new Exception("Module is not a type type.");

        EnterComment();

        WriteDescription(module.Description);
        WriteSource(module.Source);
        WriteSeeCollection(module.See);

        WriteLine($"@class {module.Name}");

        ExitComment();

        WriteLine($"local {module.Name} = {{}}");
        WriteLine(null);

        EnterComment();

        WriteLine($"@alias {baseModule.Name}.{module.Name} {module.Name}");

        ExitComment();

        WriteLine(null);

        WriteDefinitions(module);
    }

    private void WriteDefinitions(Module module)
    {
        foreach (var definition in module.Definitions)
        {
            if (definition is Function function)
            {
                WriteFunction(module, function);
            }
            else if (definition is Variable variable)
            {
                WriteVariable(module, variable);
            }
            else
            {
                throw new ConversionNotSupportedForTypeException(definition.GetType());
            }
        }
    }

    private void WriteFunction(Module module, Function function)
    {
        EnterComment();

        WriteDescription(function.Description);
        WriteSource(function.Source);
        WriteSeeCollection(function.See);

        using var enumerator = function.CombineOverloads().GetEnumerator();
        enumerator.MoveNext();

        var firstOverload = enumerator.Current;

        while (enumerator.MoveNext())
        {
            Write("@overload fun(");
            Write(
                string.Join(
                    ", ",
                    enumerator.Current.Parameters.Select(x =>
                    {
                        var name = GetParameterLuaFullName(x);
                        var type = ConvertToLuaType(x.Type);

                        return $"{name} : {type}";
                    })
                )
            );
            Write(")");

            if (enumerator.Current.Returns.Length > 0)
            {
                Write(" : ");
                Write(
                    string.Join(
                        ", ",
                        enumerator.Current.Returns
                            .Select(x => ConvertToLuaType(x.Type))
                    )
                );
            }

            WriteLine(null);
        }

        foreach (var param in function.MergeParameters())
        {
            var name = GetParameterLuaFullName(param);
            var type = ConvertToLuaType(param.Type);

            Write($"@param {name} {type}");

            if (!string.IsNullOrWhiteSpace(param.DefaultValue))
                Write($" Default: `{param.DefaultValue}`.");

            if (!string.IsNullOrWhiteSpace(param.Description))
                Write(" " + param.Description.ReplaceLineEndings(" "));

            WriteLine(null);
        }

        foreach (var @return in firstOverload.Returns)
        {
            var type = ConvertToLuaType(@return.Type);

            WriteLine($"@return {(type)} . {@return.Description.ReplaceLineEndings(" ")}");
        }

        ExitComment();

        Write($"function {module.Name}{(function.NeedSelf ? ':' : '.')}{function.Name}(");

        if (firstOverload.Parameters.Length > 0)
            Write(string.Join(", ", firstOverload.Parameters.Select(x => x.Name)));

        WriteLine(") end");
        WriteLine(null);
    }

    private void WriteVariable(Module module, Variable variable)
    {
        EnterComment();

        WriteDescription(variable.Description);
        WriteSource(variable.Source);
        WriteSeeCollection(variable.See);

        ExitComment();

        Write($"{module.Name}.{variable.Name}");

        if (!string.IsNullOrWhiteSpace(variable.Value))
            Write($" = {variable.Value}");
        else
            Write(" = {}");

        WriteLine(null);
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
                    Write($" {see.Link}");
                else
                    Write(" .");

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
            if (_comment)
                _writer.Write("---");

            _isCursorOnNewLine = false;
        }

        if (str != null)
            _writer.Write(str);
    }

    private void EnterComment()
    {
        _comment = true;
    }

    private void ExitComment()
    {
        _comment = false;
    }

    public void Dispose()
    {
        _writer.Dispose();
    }

    private static string GetParameterLuaFullName(Parameter parameter)
    {
        var name = parameter.Name;

        if (parameter.Optional)
            name += '?';

        return name;
    }

    private static string ConvertToLuaType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            type = "any";

        type = type.Replace("function(", "fun(");
        type = Regex.Replace(type, @"{\s*([a-zA-Z_]+?)\s*}", x => $"({ConvertToLuaType(x.Groups[1].Value)})[]");
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{ConvertToLuaType(x.Groups[1].Value)}:");
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{{ [number]: {ConvertToLuaType(x.Groups[1].Value)} }}");
        type = Regex.Replace(type, @"([a-zA-Z_]+?)\.\.\.", x => $"({ConvertToLuaType(x.Groups[1].Value)})[]");

        return type;
    }
}
