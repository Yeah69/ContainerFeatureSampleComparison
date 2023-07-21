using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsConcreteClass)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.ConcreteClass;

// Simple class that we want to create objects from using a container
internal class ConcreteClass
{
}

// The type of the class needs to be registered with the container
[ImplementationAggregation(typeof(ConcreteClass))]
// Also, the container needs to have a create-function specified
[CreateFunction(typeof(ConcreteClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClass = container.Create();
        Console.WriteLine(concreteClass.GetType().Name); // ConcreteClass
    }
}