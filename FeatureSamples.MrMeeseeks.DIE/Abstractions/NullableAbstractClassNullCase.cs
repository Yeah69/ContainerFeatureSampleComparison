using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsNullableAbstractClassNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.NullableAbstractClassNullCase;

// Simple abstract class that doesn't have any implementation
internal abstract class AbstractClass
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    internal Parent(AbstractClass? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

// There is no concrete implementation of AbstractClass to register
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
        using var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: True
    }
}