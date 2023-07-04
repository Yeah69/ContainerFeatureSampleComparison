using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsExplicitPropertyChoice)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.ExplicitPropertyChoice;

internal class ConcreteClassA {}
internal class ConcreteClassB {}
internal class ConcreteClassC {}
internal class ConcreteClassD {}

internal class Parent
{
    internal ConcreteClassA? Dependency { get; init; }
    internal ConcreteClassB? DependencySet { get; set; }
    internal required ConcreteClassC DependencyRequiredSet { get; set; }
    internal required ConcreteClassD DependencyRequiredInit { get; init; }
}

[ImplementationAggregation(typeof(Parent), typeof(ConcreteClassA), typeof(ConcreteClassB), typeof(ConcreteClassC), typeof(ConcreteClassD))]
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
        // Do something with parent and/or its dependencies
    }
}