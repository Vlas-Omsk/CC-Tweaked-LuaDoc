using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public sealed class EntityWriter : IDisposable
{
    private const int _threashold = 80;
    private const string _commentPrefix = "---";
    private readonly StreamWriter _writer;

    private class Overload
    {
        public Overload(Parameter[] parameters, Return[] returns)
        {
            Parameters = parameters;
            Returns = returns;
        }

        public Parameter[] Parameters { get; }
        public Return[] Returns { get; }
    }

    public EntityWriter(string path)
    {
        _writer = new StreamWriter(path);
    }

    public void Write(IEnumerable<Module> modules)
    {
        var enumerator = modules.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new Exception();

        WriteBaseModuleEntity(enumerator.Current);

        while (enumerator.MoveNext())
        {
            WriteTypeModuleEntity(enumerator.Current);
        }
    }

    private void WriteBaseModuleEntity(Module module)
    {
        if (module.IsType)
            throw new Exception();

        WriteCommentLine("@meta");
        _writer.WriteLine();

        WriteDescription(module.Description);
        WriteSeeCollection(module.See);

        WriteCommentLine($"@class {module.Name}lib");
        _writer.WriteLine($"{module.Name} = {{}}");
        _writer.WriteLine();

        WriteDefinitions(module);
    }

    private void WriteTypeModuleEntity(Module module)
    {
        if (!module.IsType)
            throw new Exception();

        WriteDescription(module.Description);
        WriteSeeCollection(module.See);

        WriteCommentLine($"@class {module.Name}");
        _writer.WriteLine($"local {module.Name} = {{}}");
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
        WriteSeeCollection(function.See);

        using var enumerator = CombineAllOverloads(function).GetEnumerator();
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
                _writer.Write(string.Join(", ", enumerator.Current.Returns.Select(x => x.Type)));
            }

            _writer.WriteLine();
        }

        var parameters = CollectAllParameters(function);

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

        if (firstOverload != null && firstOverload.Returns.Length > 0)
        {
            foreach (var @return in firstOverload.Returns)
                WriteCommentLine($"@return {(string.IsNullOrWhiteSpace(@return.Type) ? "any" : @return.Type)} . {@return.Description.ReplaceLineEndings(" ")}");
        }

        _writer.Write($"function {module.Name}{(function.IsInstance ? ':' : '.')}{function.Name}(");

        if (firstOverload != null && firstOverload.Parameters.Length > 0)
            _writer.Write(string.Join(", ", firstOverload.Parameters.Select(x => x.Name)));

        _writer.WriteLine(") end");
        _writer.WriteLine();
    }

    private (string name, string type) GetParameterPresentation(Parameter parameter)
    {
        var name = parameter.Name;

        if (parameter.Optional)
            name += '?';

        var type = parameter.Type;

        if (string.IsNullOrWhiteSpace(type))
            type = "any";

        return (name, type);
    }

    private IEnumerable<Overload> CombineAllOverloads(Function function)
    {
        if (function.ParametersOverloads.Length > 0)
        {
            foreach (var parameters in function.ParametersOverloads)
            {
                if (function.ReturnsOverloads.Length > 0)
                {
                    foreach (var returns in function.ReturnsOverloads)
                    {
                        yield return new Overload(parameters.Items, returns.Items);
                    }
                }
                else
                {
                    yield return new Overload(parameters.Items, Array.Empty<Return>());
                }
            }
        }
        else
        {
            foreach (var returns in function.ReturnsOverloads)
            {
                yield return new Overload(Array.Empty<Parameter>(), returns.Items);
            }
        }
    }

    private IEnumerable<Parameter> CollectAllParameters(Function function)
    {
        var result = new List<Parameter>();

        foreach (var parametersOverload in function.ParametersOverloads)
        {
            foreach (var parameter in parametersOverload.Items)
            {
                var find = result.FirstOrDefault(x => x.Name == parameter.Name);

                if (find != null)
                {
                    if (
                        find.Description != parameter.Description ||
                        find.Optional != parameter.Optional ||
                        find.Type != parameter.Type
                    )
                        throw new Exception("Ambiguous match");

                    continue;
                }

                result.Add(parameter);
            }
        }

        return result.ToArray();
    }

    private void WriteSeeCollection(string[] seeCollection)
    {
        if (seeCollection != null)
        {
            foreach (var see in seeCollection)
                WriteCommentLine($"@see {see}");

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

    public void Dispose()
    {
        _writer.Dispose();
    }
}
