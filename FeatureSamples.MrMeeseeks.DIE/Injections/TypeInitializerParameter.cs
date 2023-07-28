using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsTypeInitializerParameter)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.TypeInitializerParameter;

// Simple class that we want to inject into another class
internal class ConcreteClass {}

internal class Parent
{
    internal ConcreteClass? Dependency { get; private set; }
    // The type initializer method can have dependencies which are resolved by the container as well
    internal void Initialize(ConcreteClass dependency) => Dependency = dependency;
}

[ImplementationAggregation(typeof(Parent), typeof(ConcreteClass))]
[Initializer(typeof(Parent), nameof(Parent.Initialize))]
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
        Console.WriteLine($"Is null: {parent.Dependency is null}"); // Is null: False
        Console.WriteLine(parent.Dependency!.GetType().Name); // ConcreteClass
    }
}