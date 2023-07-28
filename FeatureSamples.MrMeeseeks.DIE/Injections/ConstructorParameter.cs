using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.InjectionsConstructorParameter)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Injections.ConstructorParameter;

// Simple class that we want to inject into another class
internal class ConcreteClass
{
}

internal class Parent
{
    // Inject it here as a constructor parameter
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
        Console.WriteLine(parent.Dependency.GetType().Name); // ConcreteClass
    }
}