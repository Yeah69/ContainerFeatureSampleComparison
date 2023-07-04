using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TuplesValueTuple)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Tuples.ValueTuple;

internal class ConcreteClass {}

internal interface IInterface {}

internal class Implementation : IInterface {}

[ImplementationAggregation(typeof(ConcreteClass), typeof(Implementation))]
[CreateFunction(typeof(ValueTuple<ConcreteClass, Implementation, int>), "Create")]
internal partial class Container
{
    private Container() {}
    
    private int DIE_Factory => 42;
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var valueTuple = container.Create();
        // Do something with items
    }
}