﻿using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsStructRecord)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.StructRecord;

internal record struct StructRecord
{
}

[ImplementationAggregation(typeof(StructRecord))]
[CreateFunction(typeof(StructRecord), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var structRecord = container.Create();
        // Do something with implementation
    }
}