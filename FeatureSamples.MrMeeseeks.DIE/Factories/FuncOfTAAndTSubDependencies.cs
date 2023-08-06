using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesFuncWithParameterForSubDependencies)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Factories.FuncOfTAAndTSubDependencies;

internal class ConcreteClass
{
    // Notice the int parameter
    internal ConcreteClass(int i) { }
}

internal class Parent
{
    internal Parent(ConcreteClass child) {}
}

// Don't register the type of int
[ImplementationAggregation(typeof(Parent), typeof(ConcreteClass))]
// Make int a parameter of the Func-factory
[CreateFunction(typeof(Func<int, Parent>), "Create")]
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
        // The caller side parameter can also be used for sub-dependencies
        // In this case the Parent class doesn't require an int value, but its sub-dependency ConcreteClass does
        var parentA = concreteClassFactory(6);
        var parentB = concreteClassFactory(9);
        Console.WriteLine(parentA.GetType().Name); // Parent
        Console.WriteLine(parentB.GetType().Name); // Parent
        Console.WriteLine(parentA == parentB); // False
    }
}