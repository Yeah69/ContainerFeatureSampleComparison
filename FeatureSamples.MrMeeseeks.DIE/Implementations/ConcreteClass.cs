﻿using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsConcreteClass)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.ConcreteClass;

internal class ConcreteClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(ConcreteClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClass = container.Create();
        // Do something with implementation
    }
}