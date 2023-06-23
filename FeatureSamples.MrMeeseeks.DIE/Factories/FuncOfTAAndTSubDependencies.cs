using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesFuncWithParameterForSubDependencies)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.FuncOfTAAndTSubDependencies;

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
// Make int a parameter of the Func-Factory
[CreateFunction(typeof(Func<int, Parent>), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var concreteClassFactory = container.Create();
        var concreteClassA = concreteClassFactory(6);
        var concreteClassB = concreteClassFactory(9);
        // Do something with implementation
    }
}