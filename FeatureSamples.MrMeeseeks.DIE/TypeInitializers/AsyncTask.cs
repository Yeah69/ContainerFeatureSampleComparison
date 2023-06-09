﻿using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TypeInitializersAsyncTask)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.TypeInitializers.AsyncTask;

internal class ConcreteClass
{
    internal bool Initialized { get; private set; }
    internal async Task Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

[ImplementationAggregation(typeof(ConcreteClass))]
[Initializer(typeof(ConcreteClass), nameof(ConcreteClass.Initialize))]
[CreateFunction(typeof(ConcreteClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static async Task Use()
    {
        await using var container = Container.DIE_CreateContainer();
        var concreteClass = await container.Create();
        Console.WriteLine($"Initialized: {concreteClass.Initialized}"); // Initialized: True
    }
}