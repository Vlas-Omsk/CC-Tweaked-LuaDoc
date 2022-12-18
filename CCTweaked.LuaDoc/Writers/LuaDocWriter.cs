using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities;
using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.Writers;

public sealed class LuaDocWriter : IDocWriter
{
    private readonly LuaWriter _writer;

    public LuaDocWriter(LuaWriter writer)
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

        _writer.EnterComment();

        _writer.WriteLine("@meta");

        _writer.ExitComment();

        _writer.WriteLine(null);

        _writer.EnterComment();

        WriteDescriptionLine(module.Description);
        WriteSource(module.Source);
        WriteSeeCollection(module.See);

        _writer.WriteLine($"@class {module.Name}lib");

        _writer.ExitComment();

        _writer.WriteLine($"{module.Name} = {{}}");
        _writer.WriteLine(null);

        WriteDefinitions(module);
    }

    private void WriteTypeModule(Module baseModule, Module module)
    {
        if (module.Type != ModuleType.Type)
            throw new Exception("Module is not a type type.");

        _writer.EnterComment();

        WriteDescriptionLine(module.Description);
        WriteSource(module.Source);
        WriteSeeCollection(module.See);

        _writer.WriteLine($"@class {module.Name}");

        _writer.ExitComment();

        _writer.WriteLine($"local {module.Name} = {{}}");
        _writer.WriteLine(null);

        _writer.EnterComment();

        _writer.WriteLine($"@alias {baseModule.Name}.{module.Name} {module.Name}");

        _writer.ExitComment();

        _writer.WriteLine(null);

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
        _writer.EnterComment();

        WriteDescriptionLine(function.Description);
        WriteSource(function.Source);
        WriteSeeCollection(function.See);

        using var enumerator = function.CombineOverloads().GetEnumerator();
        enumerator.MoveNext();

        var firstOverload = enumerator.Current;

        while (enumerator.MoveNext())
        {
            _writer.Write("@overload fun(");
            _writer.Write(
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
            _writer.Write(")");

            if (enumerator.Current.Returns.Length > 0)
            {
                _writer.Write(" : ");
                _writer.Write(
                    string.Join(
                        ", ",
                        enumerator.Current.Returns
                            .Select(x => ConvertToLuaType(x.Type))
                    )
                );
            }

            _writer.WriteLine(null);
        }

        foreach (var param in function.MergeParameters())
        {
            var name = GetParameterLuaFullName(param);
            var type = ConvertToLuaType(param.Type);

            _writer.Write($"@param {name} {type}");

            if (!string.IsNullOrWhiteSpace(param.DefaultValue))
                _writer.Write($" Default: `{param.DefaultValue}`.");

            if (param.Description != null && param.Description.Any())
            {
                _writer.Write(" ");

                new LuaDescriptionWriter(_writer)
                {
                    WithoutLineEndings = true
                }.WriteDescription(param.Description);
            }

            _writer.WriteLine(null);
        }

        foreach (var @return in firstOverload.Returns)
        {
            var type = ConvertToLuaType(@return.Type);

            _writer.Write($"@return {(type)} .");

            if (@return.Description != null && @return.Description.Any())
            {
                _writer.Write(" ");
                new LuaDescriptionWriter(_writer)
                {
                    WithoutLineEndings = true
                }.WriteDescription(@return.Description);
            }

            _writer.WriteLine(null);
        }

        _writer.ExitComment();

        _writer.Write($"function {module.Name}{(function.NeedSelf ? ':' : '.')}{function.Name}(");

        if (firstOverload.Parameters.Length > 0)
            _writer.Write(string.Join(", ", firstOverload.Parameters.Select(x => x.Name)));

        _writer.WriteLine(") end");
        _writer.WriteLine(null);
    }

    private void WriteVariable(Module module, Variable variable)
    {
        _writer.EnterComment();

        WriteDescriptionLine(variable.Description);
        WriteSource(variable.Source);
        WriteSeeCollection(variable.See);

        _writer.ExitComment();

        _writer.Write($"{module.Name}.{variable.Name}");

        if (!string.IsNullOrWhiteSpace(variable.Value))
            _writer.Write($" = {variable.Value}");
        else
            _writer.Write(" = {}");

        _writer.WriteLine(null);
        _writer.WriteLine(null);
    }

    private void WriteDescriptionLine(IDescriptionNode[] description)
    {
        if (description != null && description.Any())
        {
            new LuaDescriptionWriter(_writer).WriteDescription(description);
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
                switch (see.Link.Type)
                {
                    case LinkNodeType.TypeLink:
                        _writer.Write($"@see {see.Link.Link}");
                        break;
                    case LinkNodeType.ExternalLink:
                        _writer.Write($"See: [{see.Link.Name}]({see.Link.Link})");
                        break;
                }

                if (see.Description != null && see.Description.Any())
                {
                    _writer.Write($" ");
                    new LuaDescriptionWriter(_writer)
                    {
                        WithoutLineEndings = true
                    }.WriteDescription(see.Description);
                }

                _writer.WriteLine(null);
            }
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

        // Examples:
        // { string }
        // (string)[]
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/resources/data/computercraft/lua/rom/apis/settings.lua#L173
        type = Regex.Replace(type, @"{\s*([a-zA-Z_]+?)\s*}", x => $"({ConvertToLuaType(x.Groups[1].Value)})[]");

        // Examples:
        // { [string] = string }
        // { [string]: string }
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/doc/stub/http.lua#L80
        //
        // { url = string, headers? = { [string] = string }, binary? = boolean, method? = string, redirect? = boolean }
        // { url: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean }
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/doc/stub/http.lua#L80
        type = Regex.Replace(type, @"([\[a-zA-Z\]?]+)\s*=", x => $"{ConvertToLuaType(x.Groups[1].Value)}:");

        // Examples:
        // { string... }
        // { [number]: string }
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java#L104
        type = Regex.Replace(type, @"{\s*(.+)\.\.\.\s*}", x => $"{{ [number]: {ConvertToLuaType(x.Groups[1].Value)} }}");

        // Examples:
        // string...
        // (string)...
        // https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/7f89fc716868d710fa394efde7ee498fab7b0fee/src/main/resources/data/computercraft/lua/rom/apis/peripheral.lua#L155
        type = Regex.Replace(type, @"([a-zA-Z_]+?)\.\.\.", x => $"({ConvertToLuaType(x.Groups[1].Value)})...");

        return type;
    }
}
