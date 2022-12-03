using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc.Writers;

public sealed class LuaDocWriter : IDocWriter, IDisposable
{
    private const int _threashold = 80;
    private const string _commentPrefix = "---";
    private readonly TextWriter _writer;

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
        if (module.IsType)
            throw new Exception();

        WriteCommentLine("@meta");
        _writer.WriteLine();

        WriteDescription(module.Description);
        WriteSource(module.Source);
        WriteSeeCollection(module.See);

        WriteCommentLine($"@class {module.Name}lib");
        _writer.WriteLine($"{module.Name} = {{}}");
        _writer.WriteLine();

        WriteDefinitions(module);
    }

    private void WriteTypeModule(Module baseModule, Module module)
    {
        if (!module.IsType)
            throw new Exception();

        WriteDescription(module.Description);
        WriteSource(module.Source);
        WriteSeeCollection(module.See);

        WriteCommentLine($"@class {module.Name}");
        _writer.WriteLine($"local {module.Name} = {{}}");
        _writer.WriteLine();
        WriteCommentLine($"@alias {baseModule.Name}.{module.Name} {module.Name}");
        _writer.WriteLine();

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
                throw new Exception();
            }
        }
    }

    private void WriteVariable(Module module, Variable variable)
    {
        WriteDescription(variable.Description);
        WriteSource(variable.Source);
        WriteSeeCollection(variable.See);

        _writer.Write($"{module.Name}.{variable.Name}");

        if (!string.IsNullOrWhiteSpace(variable.Value))
            _writer.Write($" = {variable.Value}");
        else
            _writer.Write(" = {}");

        _writer.WriteLine();
        _writer.WriteLine();
    }

    private void WriteFunction(Module module, Function function)
    {
        WriteDescription(function.Description);
        WriteSource(function.Source);
        WriteSeeCollection(function.See);

        using var enumerator = function.CombineAllOverloads().GetEnumerator();
        enumerator.MoveNext();

        var firstOverload = enumerator.Current;

        while (enumerator.MoveNext())
        {
            WriteComment("@overload fun(");
            _writer.Write(string.Join(", ", enumerator.Current.Parameters.Select(x =>
            {
                var (name, type) = GetParameterPresentation(x);

                return $"{name} : {type}";
            })));
            _writer.Write(")");

            if (enumerator.Current.Returns.Length > 0)
            {
                _writer.Write(" : ");
                _writer.Write(string.Join(", ", enumerator.Current.Returns.Select(x => ConvertToLuaType(x.Type))));
            }

            _writer.WriteLine();
        }

        var parameters = function.CollectAllParameters();

        foreach (var param in parameters)
        {
            var (name, type) = GetParameterPresentation(param);

            WriteComment($"@param {name} {type}");

            if (!string.IsNullOrWhiteSpace(param.DefaultValue))
                _writer.Write($" Default: `{param.DefaultValue}`.");

            if (!string.IsNullOrWhiteSpace(param.Description))
                _writer.Write(" " + param.Description.ReplaceLineEndings(" "));

            _writer.WriteLine();
        }

        if (firstOverload.Returns.Length > 0)
        {
            foreach (var @return in firstOverload.Returns)
            {
                var type = ConvertToLuaType(@return.Type);

                WriteCommentLine($"@return {(type)} . {@return.Description.ReplaceLineEndings(" ")}");
            }
        }

        _writer.Write($"function {module.Name}{(function.IsInstance ? ':' : '.')}{function.Name}(");

        if (firstOverload.Parameters.Length > 0)
            _writer.Write(string.Join(", ", firstOverload.Parameters.Select(x => x.Name)));

        _writer.WriteLine(") end");
        _writer.WriteLine();
    }

    private (string name, string type) GetParameterPresentation(Parameter parameter)
    {
        var name = parameter.Name;

        if (parameter.Optional)
            name += '?';

        var type = ConvertToLuaType(parameter.Type);

        return (name, type);
    }

    private void WriteSeeCollection(See[] seeCollection)
    {
        if (seeCollection != null)
        {
            foreach (var see in seeCollection)
                WriteCommentLine($"@see {see}");

            foreach (var see in seeCollection)
            {
                WriteComment($"@see");

                if (!string.IsNullOrWhiteSpace(see.Link))
                    WriteComment($" {see.Link}");
                else
                    WriteComment(" .");

                if (!string.IsNullOrWhiteSpace(see.Description))
                    WriteComment(see.Description);

                WriteCommentLine(null);
            }

            WriteCommentLine(null);
        }
    }

    private void WriteSource(string source)
    {
        if (!string.IsNullOrWhiteSpace(source))
        {
            WriteCommentLine($"[View source]({source})");
            WriteCommentLine(null);
        }
    }

    private void WriteDescription(string description)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            WriteCommentText(description);
            WriteCommentLine(null);
        }
    }

    private void WriteCommentText(string text)
    {
        var lines = text.Split(Environment.NewLine);

        foreach (var currentLine in lines)
        {
            var words = currentLine.Split(' ');
            string line = null;

            foreach (var word in words)
            {
                if ((line?.Length ?? 0) + word.Length + 1 > _threashold)
                {
                    WriteCommentLine(line);
                    line = word;
                }
                else
                {
                    if (line == null)
                        line = word;
                    else
                        line += ' ' + word;
                }
            }

            WriteCommentLine(line);
        }
    }

    private void WriteCommentLine(string line)
    {
        WriteComment(line);
        _writer.WriteLine();
    }

    private void WriteComment(string line)
    {
        _writer.Write(_commentPrefix);
        if (line != null)
            _writer.Write(line.ReplaceLineEndings(" "));
    }

    private static string ConvertToLuaType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            type = "any";

        type = type.Replace("function(", "fun(");
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{x.Groups[1].Value}:");
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{{ [number]: {x.Groups[1].Value} }}");

        return type;
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
