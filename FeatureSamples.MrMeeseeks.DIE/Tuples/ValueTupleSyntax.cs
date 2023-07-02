using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TuplesValueTupleSyntax)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Tuples.ValueTupleSyntax;

internal class ConcreteClass {}

internal interface IInterface {}

internal class Implementation : IInterface {}

[ImplementationAggregation(typeof(ConcreteClass), typeof(Implementation))]
[CreateFunction(typeof((ConcreteClass, Implementation, int)), "Create")]
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
        var (concreteClass, implementation, number) = container.Create();
        // Do something with items
    }
}