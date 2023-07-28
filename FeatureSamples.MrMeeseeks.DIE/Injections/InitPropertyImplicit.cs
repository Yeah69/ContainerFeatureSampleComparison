using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsInitPropertyImplicit)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.InitPropertyImplicit;

// Simple classes that we want to inject into another class
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
// No need for explicit registration of properties (like in the "ExplicitPropertyChoice"-sample) to be injected, because they are all init-properties.
// The rationale for this convention is: if the container is instantiating the Parent-object, then the user has no chance to set the init-properties themselves.
// That means it would be strange not have the container inject them.
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
        Console.WriteLine(parent.Dependency!.GetType().Name); // ConcreteClassA
        Console.WriteLine(parent.DependencyRequired.GetType().Name); // ConcreteClassB
    }
}