using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsInterfaceSingleImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.InterfaceSingleImplementation;

// Simple interface that will work as an abstraction for an implementation
internal interface IInterface
{
}

// Simple class that will be used as an implementation for the interface
internal class ConcreteClass : IInterface
{
}

// Register implementation type only
// No explicit mapping to the interface is needed since it is the only implementation
[ImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(IInterface), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var instance = container.Create();
        Console.WriteLine(instance.GetType().Name); // ConcreteClass
    }
}