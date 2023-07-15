using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.MiscRegisterAssemblyImplementations)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Misc.RegisterAssemblyImplementations;

// No explicit aggregation of implementations is required.
// Just the following attribute is enough to register all implementations of the owning assemblies of the passed types.
[AssemblyImplementationsAggregation(typeof(DateTime))]

// Create-functions for types of a referenced assembly
[ConstructorChoice(typeof(DateTime))]
[ConstructorChoice(typeof(DateOnly))]
[ConstructorChoice(typeof(TimeOnly))]
[CreateFunction(typeof(DateTime), "CreateDateTime")]
[CreateFunction(typeof(DateOnly), "CreateDateOnly")]
[CreateFunction(typeof(TimeOnly), "CreateTimeOnly")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var dateTime = container.CreateDateTime();
        var dateOnly = container.CreateDateOnly();
        var timeOnly = container.CreateDateOnly();
        // Do something with the created instances
    }
}