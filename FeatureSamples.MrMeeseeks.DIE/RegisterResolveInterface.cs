using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsInterface)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.RegisterResolveInterface;

internal interface IInterface
{
}

internal class Implementation : IInterface
{
}

[ImplementationAggregation(typeof(Implementation))]
[ImplementationChoice(typeof(IInterface), typeof(Implementation))]
[CreateFunction(typeof(IInterface), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var instance = container.Create();
        // Do something with implementation
    }
}