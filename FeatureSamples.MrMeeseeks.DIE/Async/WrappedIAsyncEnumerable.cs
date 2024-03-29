﻿using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AsyncWrappedIAsyncEnumerable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Async.WrappedIAsyncEnumerable;

internal interface IInterface
{
    bool Initialized { get; }
}

// Simple class that we want to inject into another class and which is initialized asynchronously
internal class ConcreteClassTask : IInterface
{
    public bool Initialized { get; private set; }
    internal async Task Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

// Another simple class that we want to inject into another class and which is initialized asynchronously (this time with ValueTask)
internal class ConcreteClassValueTask : IInterface
{
    public bool Initialized { get; private set; }
    internal async ValueTask Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

internal class Parent
{
    // Injecting the simple classes wrapped in an IAsyncEnumerable<T>
    internal Parent(IAsyncEnumerable<IInterface> dependencies) => Dependencies = dependencies;
    internal IAsyncEnumerable<IInterface> Dependencies { get; }
}

[ImplementationAggregation(typeof(Parent), typeof(ConcreteClassTask), typeof(ConcreteClassValueTask))]
[Initializer(typeof(ConcreteClassTask), nameof(ConcreteClassTask.Initialize))]
[Initializer(typeof(ConcreteClassValueTask), nameof(ConcreteClassValueTask.Initialize))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static async Task Use()
    {
        await using var container = Container.DIE_CreateContainer();
        // Notice that the Create-function stays synchronous, because the injection of the simple classes is wrapped in an IAsyncEnumerable<T>.
        var parent = container.Create();
        await foreach (var dependency in parent.Dependencies)
        {
            Console.WriteLine($"Initialized: {dependency.Initialized}"); // Initialized: True
        }
    }
}