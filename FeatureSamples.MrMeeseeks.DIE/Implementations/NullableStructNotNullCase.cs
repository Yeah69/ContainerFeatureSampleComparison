using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsNullableStructNotNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.NullableStructNotNullCase;

internal struct Struct
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    internal Parent(Struct? child) => IsNull = !child.HasValue; 
    internal bool IsNull { get; }
}

// Register the Struct as an implementation
[ImplementationAggregation(typeof(Parent), typeof(Struct))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: False
    }
}