using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AbstractionsNullableInterfaceNotNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Abstractions.NullableInterfaceNotNullCase;

internal interface IInterface
{
}

internal class ConcreteClass : IInterface
{
}

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
        var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: False
    }
}