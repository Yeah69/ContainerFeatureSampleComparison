using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.MiscRegisterAllImplementations)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Misc.AllImplementations;

// No explicit aggregation of implementations is required.
// Just the following attribute is enough to register all implementations of the current assembly and all referenced assemblies.
[AllImplementationsAggregation]

// Create-functions for types of a referenced assembly
[ConstructorChoice(typeof(DateTime))]
[ConstructorChoice(typeof(DateOnly))]
[ConstructorChoice(typeof(TimeOnly))]
[CreateFunction(typeof(DateTime), "CreateDateTime")]
[CreateFunction(typeof(DateOnly), "CreateDateOnly")]
[CreateFunction(typeof(TimeOnly), "CreateTimeOnly")]

// Create-function for a type of another namespace in the same assembly
[CreateFunction(typeof(Implementations.ConcreteClass.ConcreteClass), "CreateConcreteClass")]
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
        var concreteClass = container.CreateConcreteClass();
        // Do something with the created instances
    }
}