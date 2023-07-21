using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsStructRecord)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.StructRecord;

// Simple struct-record that we want to create objects from using a container
internal record struct StructRecord
{
}

// The type of the struct-record needs to be registered with the container
[ImplementationAggregation(typeof(StructRecord))]
// Also, the container needs to have a create-function specified
[CreateFunction(typeof(StructRecord), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var structRecord = container.Create();
        Console.WriteLine(structRecord.GetType().Name); // StructRecord
    }
}