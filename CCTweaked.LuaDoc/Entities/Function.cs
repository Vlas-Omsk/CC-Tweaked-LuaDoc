namespace CCTweaked.LuaDoc.Entities;

public sealed class Function : Entity, IDefinition
{
    public string Name { get; set; }
    public OverloadCollection<Parameter>[] ParametersOverloads { get; set; }
    public OverloadCollection<Return>[] ReturnsOverloads { get; set; }
    public bool IsInstance { get; set; }

    public IEnumerable<Overload> CombineAllOverloads()
    {
        if (ParametersOverloads.Length > 0)
        {
            foreach (var parameters in ParametersOverloads)
            {
                if (ReturnsOverloads.Length > 0)
                {
                    foreach (var returns in ReturnsOverloads)
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
        else if (ReturnsOverloads.Length > 0)
        {
            foreach (var returns in ReturnsOverloads)
            {
                yield return new Overload(Array.Empty<Parameter>(), returns.Items);
            }
        }
        else
        {
            yield return new Overload(Array.Empty<Parameter>(), Array.Empty<Return>());
        }
    }

    public IEnumerable<Parameter> CollectAllParameters()
    {
        var result = new List<Parameter>();

        foreach (var parametersOverload in ParametersOverloads)
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
}

public sealed class OverloadCollection<T>
{
    public T[] Items { get; set; }
}

public sealed class Parameter
{
    public string Name { get; set; }
    public bool Optional { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
}

public sealed class Return
{
    public string Type { get; set; }
    public string Description { get; set; }
}