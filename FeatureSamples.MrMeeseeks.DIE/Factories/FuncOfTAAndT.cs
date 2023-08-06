using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesFuncWithParameter)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Factories.FuncOfTAAndT;

internal class ConcreteClass
{
    // Notice the int parameter
    internal ConcreteClass(int i) { }
}

// Don't register the type of int
[ImplementationAggregation(typeof(ConcreteClass))]
// Make int a parameter of the Func-factory
[CreateFunction(typeof(Func<int, ConcreteClass>), "Create")]
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
        // Additionally to the deferred creation, the factory now also takes a parameter from the caller side
        // That means the int-parameter won't be resolved from the container, but is determined by the caller
        var concreteClassA = concreteClassFactory(6);
        var concreteClassB = concreteClassFactory(9);
        Console.WriteLine(concreteClassA.GetType().Name); // ConcreteClass
        Console.WriteLine(concreteClassB.GetType().Name); // ConcreteClass
        Console.WriteLine(concreteClassA == concreteClassB); // False
    }
}