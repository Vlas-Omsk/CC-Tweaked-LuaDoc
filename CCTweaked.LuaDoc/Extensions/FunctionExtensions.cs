using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public static class FunctionExtensions
{
    public static IEnumerable<Overload> CombineOverloads(this Function function)
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
        else if (function.ReturnsOverloads.Length > 0)
        {
            foreach (var returns in function.ReturnsOverloads)
            {
                yield return new Overload(Array.Empty<Parameter>(), returns.Items);
            }
        }
        else
        {
            yield return new Overload(Array.Empty<Parameter>(), Array.Empty<Return>());
        }
    }

    public static IEnumerable<Parameter> MergeParameters(this Function function)
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
                        throw new Exception("Found parameters with the same name but different data.");

                    continue;
                }

                result.Add(parameter);
            }
        }

        return result.ToArray();
    }

    public static IEnumerable<OverloadWithMergedReturns> CombineOverloadsWithMergedReturns(this Function function)
    {
        if (function.ParametersOverloads.Length > 0)
        {
            var returns = AlignReturns(function).ToArray();

            foreach (var parameters in function.ParametersOverloads)
            {
                if (function.ReturnsOverloads.Length > 0)
                {
                    yield return new OverloadWithMergedReturns(parameters.Items, returns);
                }
                else
                {
                    yield return new OverloadWithMergedReturns(parameters.Items, Array.Empty<Return[]>());
                }
            }
        }
        else if (function.ReturnsOverloads.Length > 0)
        {
            yield return new OverloadWithMergedReturns(Array.Empty<Parameter>(), AlignReturns(function).ToArray());
        }
        else
        {
            yield return new OverloadWithMergedReturns(Array.Empty<Parameter>(), Array.Empty<Return[]>());
        }
    }

    private static IEnumerable<Return[]> AlignReturns(Function function)
    {
        if (function.ReturnsOverloads.Length == 0)
            yield break;

        var maxLength = function.ReturnsOverloads.Max(x => x.Items.Length);

        foreach (var returns in function.ReturnsOverloads)
        {
            var result = new Return[maxLength];

            for (var i = 0; i < maxLength; i++)
            {
                if (i < returns.Items.Length)
                {
                    result[i] = returns.Items[i];
                }
                else
                {
                    result[i] = new Return()
                    {
                        Type = "nil"
                    };
                }
            }

            yield return result;
        }
    }
}
