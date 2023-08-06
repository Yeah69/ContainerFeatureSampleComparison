using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesLazy)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Factories.LazyOfT;

internal class ConcreteClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
// Instead of returning the implementation type directly, return a Lazy<ConcreteClass>
// which is kind of a factory considering that the Value property creates an instance of the implementation type
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
        // Lazy objects defer the time of creation of the instance to when the Value property is called
        // Contrary to Func-factories, Lazy-factories only create one instance and can't take parameters
        var concreteClass = concreteClassFactory.Value;
        Console.WriteLine(concreteClass.GetType().Name); // ConcreteClass
    }
}