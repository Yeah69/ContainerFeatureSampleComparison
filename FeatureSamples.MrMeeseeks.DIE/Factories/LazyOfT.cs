using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesLazy)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.LazyOfT;

internal class ConcreteClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(Lazy<ConcreteClass>), "Create")]
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
        var concreteClass = concreteClassFactory.Value;
        // Do something with implementation
    }
}