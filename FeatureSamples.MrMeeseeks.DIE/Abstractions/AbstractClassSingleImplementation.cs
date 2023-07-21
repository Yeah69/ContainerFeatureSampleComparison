using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsAbstractClassSingleImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.AbstractClassSingleImplementation;

// Simple abstract class that will work as an abstraction for a concrete class
internal abstract class AbstractClass
{
}

// Simple class that will be used as an implementation for the abstract class
internal class ConcreteClass : AbstractClass
{
}

// Register implementation type only
// No explicit mapping to the abstract class is needed since it is the only implementation
[ImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(AbstractClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var abstractClass = container.Create();
        Console.WriteLine(abstractClass.GetType().Name); // ConcreteClass
    }
}