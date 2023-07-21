using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsAbstractClassMultipleImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.AbstractClassMultipleImplementation;

// Simple abstract class that will work as an abstraction for a concrete class
internal abstract class AbstractClass
{
}

// Simple class that will be used as an implementation for the abstract class
internal class ConcreteClass : AbstractClass
{
}

// Another simple class that implements the abstract class
internal class AnotherConcreteClass : AbstractClass
{
}

// Registering both implementation types leads to an ambiguity which implementation to choose for the abstract class
[ImplementationAggregation(typeof(ConcreteClass), typeof(AnotherConcreteClass))]
// Explicitly choose ConcreteClass as the implementation for AbstractClass
[ImplementationChoice(typeof(AbstractClass), typeof(ConcreteClass))]
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