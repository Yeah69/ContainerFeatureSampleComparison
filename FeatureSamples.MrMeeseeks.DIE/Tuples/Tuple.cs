using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TuplesTuple)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Tuples.Tuple;

internal class ConcreteClass {}

internal interface IInterface {}

internal class Implementation : IInterface {}

[ImplementationAggregation(typeof(ConcreteClass), typeof(Implementation))]
[CreateFunction(typeof(Tuple<ConcreteClass, Implementation, int>), "Create")]
internal partial class Container
{
    private Container() {}
    
    private int DIE_Factory => 42;
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var tuple = container.Create();
        // Do something with items
    }
}