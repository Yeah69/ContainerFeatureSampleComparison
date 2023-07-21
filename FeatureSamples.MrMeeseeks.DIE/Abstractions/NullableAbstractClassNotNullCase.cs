using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsNullableAbstractClassNotNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.NullableAbstractClassNotNullCase;

// Simple abstract class that will work as an abstraction for a concrete class
internal abstract class AbstractClass
{
}

// Simple class that will be used as an implementation for the abstract class
internal class ConcreteClass : AbstractClass
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    internal Parent(AbstractClass? child) => IsNull = child is null; 
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