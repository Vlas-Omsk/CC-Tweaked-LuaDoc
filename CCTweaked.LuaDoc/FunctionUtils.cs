using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

public static class FunctionUtils
{
    public static IEnumerable<MergedOverload> CombineMergedOverloads(Function function)
    {
        if (function.ParametersOverloads.Length > 0)
        {
            var returns = AlignReturns(function.ReturnsOverloads).ToArray();

            foreach (var parameters in function.ParametersOverloads)
            {
                if (function.ReturnsOverloads.Length > 0)
                {
                    yield return new MergedOverload(parameters.Items, returns);
                }
                else
                {
                    yield return new MergedOverload(parameters.Items, Array.Empty<Return[]>());
                }
            }
        }
        else if (function.ReturnsOverloads.Length > 0)
        {
            yield return new MergedOverload(Array.Empty<Parameter>(), AlignReturns(function.ReturnsOverloads).ToArray());
        }
        else
        {
            yield return new MergedOverload(Array.Empty<Parameter>(), Array.Empty<Return[]>());
        }
    }

    private static IEnumerable<Return[]> AlignReturns(ItemsOverload<Return>[] returnsOverloads)
    {
        if (returnsOverloads.Length == 0)
            yield break;

        var maxLength = returnsOverloads.Max(x => x.Items.Length);

        foreach (var returns in returnsOverloads)
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
