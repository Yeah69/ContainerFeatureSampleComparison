using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsNullableConcreteClassNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.NullableConcreteClassNullCase;

internal class ConcreteClass
{
}

internal class Parent
{
    internal Parent(ConcreteClass? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

// Don't register the ConcreteClass as an implementation
[ImplementationAggregation(typeof(Parent))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: True
    }
}