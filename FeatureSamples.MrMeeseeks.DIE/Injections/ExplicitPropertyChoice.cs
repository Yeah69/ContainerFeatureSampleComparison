using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsExplicitPropertyChoice)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.ExplicitPropertyChoice;

// Simple classes that we want to inject into another class
internal class ConcreteClassA {}
internal class ConcreteClassB {}
internal class ConcreteClassC {}
internal class ConcreteClassD {}

internal class Parent
{
    // Possibly inject ConcreteClassA here as an nullable init-property (but we'll later choose not to inject it).
    // The benefit of an init-property is that it can only be set by the container.
    internal ConcreteClassA? Dependency { get; init; }
    // Possibly inject ConcreteClassB here as an nullable set-property
    internal ConcreteClassB? DependencySet { get; set; }
    // Inject ConcreteClassC here as a required set-property
    internal required ConcreteClassC DependencyRequiredSet { get; set; }
    // Inject ConcreteClassA here as a required nullable init-property.
    // The benefit of an init-property is that it can only be set by the container.
    internal required ConcreteClassD DependencyRequiredInit { get; init; }
}

[ImplementationAggregation(typeof(Parent), typeof(ConcreteClassA), typeof(ConcreteClassB), typeof(ConcreteClassC), typeof(ConcreteClassD))]
// Explicitly choose which properties should be injected.
// The "PropertyChoice" configuration is only required if there are set-properties to be injected or if the set of injected properties should be limited.
// In the default case, all init-properties are injected automatically (see "InitPropertyImplicit"-sample).
[PropertyChoice(typeof(Parent), nameof(Parent.DependencySet), nameof(Parent.DependencyRequiredSet), nameof(Parent.DependencyRequiredInit))]
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
        Console.WriteLine($"Parent.Dependency is null: {parent.Dependency is null}"); // Parent.Dependency is null: True
        Console.WriteLine($"Parent.DependencySet is null: {parent.DependencySet is null}"); // Parent.DependencySet is null: False
        Console.WriteLine(parent.DependencySet!.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyRequiredSet.GetType().Name); // ConcreteClassC
        Console.WriteLine(parent.DependencyRequiredInit.GetType().Name); // ConcreteClassD
    }
}