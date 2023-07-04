using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesFunc)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.FuncOfT;

internal class ConcreteClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
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
        var concreteClassA = concreteClassFactory();
        var concreteClassB = concreteClassFactory();
        // Do something with implementation
    }
}