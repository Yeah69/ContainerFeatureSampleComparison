using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesFunc)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.FuncOfT;

internal class ConcreteClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
// Instead of returning the implementation type directly, return a factory function that creates instances of the implementation type
[CreateFunction(typeof(Func<ConcreteClass>), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClassFactory = container.Create();
        // Factories defer the time of creation of the instance to when the factory is called
        // Also each call to the factory creates a new instance (as long as the implementation type is not scoped)
        var concreteClassA = concreteClassFactory();
        var concreteClassB = concreteClassFactory();
        Console.WriteLine(concreteClassA.GetType().Name); // ConcreteClass
        Console.WriteLine(concreteClassB.GetType().Name); // ConcreteClass
        Console.WriteLine(concreteClassA == concreteClassB); // False
    }
}