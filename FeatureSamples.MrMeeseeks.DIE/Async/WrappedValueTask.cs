using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AsyncWrappedValueTask)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Async.WrappedValueTask;

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
    // Both of the simple classes are injected wrapped into ValueTask<T>
    internal Parent(ValueTask<ConcreteClassTask> taskBasedDependency, ValueTask<ConcreteClassValueTask> valueTaskBasedDependency)
    {
        TaskBasedDependency = taskBasedDependency;
        ValueTaskBasedDependency = valueTaskBasedDependency;
    }
    
    internal ValueTask<ConcreteClassTask> TaskBasedDependency { get; }
    internal ValueTask<ConcreteClassValueTask> ValueTaskBasedDependency { get; }
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
        // Notice that the Create-function stays synchronous, because the injections of the simple classes are wrapped in a ValueTask<T>.
        var parent = container.Create();
        Console.WriteLine($"Initialized: {(await parent.TaskBasedDependency).Initialized}"); // Initialized: True
        Console.WriteLine($"Initialized: {(await parent.ValueTaskBasedDependency).Initialized}"); // Initialized: True
    }
}