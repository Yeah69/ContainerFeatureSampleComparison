﻿using System.Collections.ObjectModel;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.IterablesReadOnlyCollection)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Iterables.ReadOnlyCollectionOfT;

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
[CreateFunction(typeof(ReadOnlyCollection<IInterface>), "Create")]
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