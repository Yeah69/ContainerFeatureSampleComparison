﻿using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.IterablesArray)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Iterables.ArrayOfT;

internal interface IInterface
{
}

internal class ConcreteClass : IInterface
{
}

internal class Struct : IInterface
{
}

internal record Record : IInterface;

internal record struct RecordStruct : IInterface;

[ImplementationAggregation(typeof(ConcreteClass), typeof(Struct), typeof(Record), typeof(RecordStruct))]
[CreateFunction(typeof(IInterface[]), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var iterable = container.Create(); // ConcreteClass, Struct, Record, RecordStruct
        // Do something with implementation
    }
}