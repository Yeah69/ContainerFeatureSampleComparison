using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsInitPropertyImplicit)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.InitPropertyImplicit;

internal class ConcreteClassA { }

internal class ConcreteClassB { }

internal class Parent
{
    // Non-required properties of reference types need to be nullable (C# requirement for nullability feature)
    internal ConcreteClassA? Dependency { get; init; }
    // Required properties don't have to be nullable
    internal required ConcreteClassB DependencyRequired { get; init; }
}

[ImplementationAggregation(typeof(Parent), typeof(ConcreteClassA), typeof(ConcreteClassB))]
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
        // Do something with parent and/or its dependencies
    }
}