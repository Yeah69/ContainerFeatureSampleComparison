using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AsyncNotWrapped)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Async.NotWrapped;

// Simple class that we want to inject into another class and which is initialized asynchronously
internal class ConcreteClassTask
{
    internal bool Initialized { get; private set; }
    internal async Task Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

// Another simple class that we want to inject into another class and which is initialized asynchronously (this time with ValueTask)
internal class ConcreteClassValueTask
{
    internal bool Initialized { get; private set; }
    internal async ValueTask Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

internal class Parent
{
    // Both of the simple classes are injected without being wrapped in a Task<T> or ValueTask<T>
    internal Parent(ConcreteClassTask taskBasedDependency, ConcreteClassValueTask valueTaskBasedDependency)
    {
        TaskBasedDependency = taskBasedDependency;
        ValueTaskBasedDependency = valueTaskBasedDependency;
    }
    
    internal ConcreteClassTask TaskBasedDependency { get; }
    internal ConcreteClassValueTask ValueTaskBasedDependency { get; }
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
        // Notice that the Create-function returns a ValueTask<Parent> and not a Parent.
        // This is due to the necessary await in the Create-function, because the simple classes are initialized asynchronously but injected synchronously (not wrapped).
        var parent = await container.Create();
        Console.WriteLine($"Initialized: {parent.TaskBasedDependency.Initialized}"); // Initialized: True
        Console.WriteLine($"Initialized: {parent.ValueTaskBasedDependency.Initialized}"); // Initialized: True
    }
}