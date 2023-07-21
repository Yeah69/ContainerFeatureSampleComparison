using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsRecord)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.Record;

// Simple record that we want to create objects from using a container
internal record Record
{
}

// The type of the record needs to be registered with the container
[ImplementationAggregation(typeof(Record))]
// Also, the container needs to have a create-function specified
[CreateFunction(typeof(Record), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var record = container.Create();
        Console.WriteLine(record.GetType().Name); // Record
    }
}