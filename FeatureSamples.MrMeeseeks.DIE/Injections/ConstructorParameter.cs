using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsConstructorParameter)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.ConstructorParameter;

internal class ConcreteClass
{
}

internal class Parent
{
    internal Parent(ConcreteClass child) => Dependency = child;
    internal ConcreteClass Dependency { get; }
}

[ImplementationAggregation(typeof(Parent), typeof(ConcreteClass))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        // Do something with parent and/or its dependency
    }
}