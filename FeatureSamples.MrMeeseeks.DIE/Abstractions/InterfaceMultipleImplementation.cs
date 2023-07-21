using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsInterfaceMultipleImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.InterfaceMultipleImplementation;

// Simple interface that will work as an abstraction for an implementation
internal interface IInterface
{
}

// Simple class that will be used as an implementation for the interface
internal class ConcreteClass : IInterface
{
}

// Another simple class that implements the interface
internal class AnotherConcreteClass : IInterface
{
}

// Registering both implementation types leads to an ambiguity which implementation to choose for the interface
[ImplementationAggregation(typeof(ConcreteClass), typeof(AnotherConcreteClass))]
// Explicitly choose ConcreteClass as the implementation for IInterface
[ImplementationChoice(typeof(IInterface), typeof(ConcreteClass))]
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