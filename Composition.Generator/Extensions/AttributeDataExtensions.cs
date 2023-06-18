using System.Linq;
using Microsoft.CodeAnalysis;

namespace ContainerFeatureSampleComparison.SampleAggregationGenerator.Extensions;

public static class AttributeDataExtensions
{
    public static bool TryGetNamedArgument(this AttributeData attributeData, string name, out TypedConstant value)
    {
        foreach (var namedArgument in attributeData.NamedArguments.Where(namedArgument => namedArgument.Key == name))
        {
            value = namedArgument.Value;
            return true;
        }

        value = default;
        return false;
    }
}