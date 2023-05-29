using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.RegisterResolveAbstractClass)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.RegisterResolveAbstractClass;

internal abstract class AbstractClass
{
}

internal class ConcreteClass : AbstractClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
[ImplementationChoice(typeof(AbstractClass), typeof(ConcreteClass))]
[CreateFunction(typeof(AbstractClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var abstractClass = container.Create();
        // Do something with implementation
    }
}