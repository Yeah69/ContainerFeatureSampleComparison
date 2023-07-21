using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsStruct)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.Struct;

// Simple struct that we want to create values from using a container
internal struct Struct
{
}

// The type of the struct needs to be registered with the container
[ImplementationAggregation(typeof(Struct))]
// Also, the container needs to have a create-function specified
[CreateFunction(typeof(Struct), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var value = container.Create();
        Console.WriteLine(value.GetType().Name); // Struct
    }
}