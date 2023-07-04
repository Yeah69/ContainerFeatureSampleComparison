using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AsyncWrappedTask)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Async.WrappedTask;

internal class ConcreteClassTask
{
    internal bool Initialized { get; private set; }
    internal async Task Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

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
    internal Parent(Task<ConcreteClassTask> taskBasedDependency, Task<ConcreteClassValueTask> valueTaskBasedDependency)
    {
        TaskBasedDependency = taskBasedDependency;
        ValueTaskBasedDependency = valueTaskBasedDependency;
    }
    
    internal Task<ConcreteClassTask> TaskBasedDependency { get; }
    internal Task<ConcreteClassValueTask> ValueTaskBasedDependency { get; }
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
        var parent = container.Create();
        Console.WriteLine($"Initialized: {(await parent.TaskBasedDependency).Initialized}"); // Initialized: True
        Console.WriteLine($"Initialized: {(await parent.ValueTaskBasedDependency).Initialized}"); // Initialized: True
    }
}