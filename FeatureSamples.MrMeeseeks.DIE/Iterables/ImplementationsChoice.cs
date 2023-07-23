using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.IterablesImplementationsChoice)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Iterables.ImplementationsChoice;

// An interface and its implementations
internal interface IInterface
{
}

internal class ConcreteClass : IInterface
{
}

internal struct Struct : IInterface
{
}

internal record Record : IInterface;

internal record struct RecordStruct : IInterface;

// Register the implementations
[ImplementationAggregation(typeof(ConcreteClass), typeof(Struct), typeof(Record), typeof(RecordStruct))]
// With following attribute the set of implementations for iterable resolutions is reduced to ConcreteClass, Record
// That may be practical in combination of convenience functions like automatically registration all implementations at once
[ImplementationCollectionChoice(typeof(IInterface), typeof(ConcreteClass), typeof(Record))]
[CreateFunction(typeof(IEnumerable<IInterface>), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var iterable = container.Create();
        foreach (var implementation in iterable)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
    }
}