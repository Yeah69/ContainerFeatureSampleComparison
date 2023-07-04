using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AsyncNotWrappedIEnumerable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Async.NotWrappedIEnumerable;

internal interface IInterface
{
    bool Initialized { get; }
}

internal class ConcreteClassTask : IInterface
{
    public bool Initialized { get; private set; }
    internal async Task Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

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
    internal Parent(IEnumerable<IInterface> dependencies) => Dependencies = dependencies;
    internal IEnumerable<IInterface> Dependencies { get; }
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
        var container = Container.DIE_CreateContainer();
        var parent = await container.Create();
        foreach (var dependency in parent.Dependencies)
        {
            Console.WriteLine($"Initialized: {dependency.Initialized}"); // Initialized: True
        }
    }
}