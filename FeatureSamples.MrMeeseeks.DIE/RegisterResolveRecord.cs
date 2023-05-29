using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.RegisterResolveRecord)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.RegisterResolveRecord;

internal record Record
{
}

[ImplementationAggregation(typeof(Record))]
[CreateFunction(typeof(Record), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var record = container.Create();
        // Do something with implementation
    }
}