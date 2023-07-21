using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsNullableInterfaceNotNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.NullableInterfaceNotNullCase;

// Simple interface that will work as an abstraction for a concrete class
internal interface IInterface
{
}

// Simple class that will be used as an implementation for the interface
internal class ConcreteClass : IInterface
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    internal Parent(IInterface? child) => IsNull = child is null; 
    internal bool IsNull { get; }
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
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: False
    }
}