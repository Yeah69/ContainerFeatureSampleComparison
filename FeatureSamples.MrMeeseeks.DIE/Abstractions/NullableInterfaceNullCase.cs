using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsNullableInterfaceNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.NullableInterfaceNullCase;

// Simple interface that doesn't have any implementation
internal interface IInterface
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    internal Parent(IInterface? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

// There is no concrete implementation of IInterface to register
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