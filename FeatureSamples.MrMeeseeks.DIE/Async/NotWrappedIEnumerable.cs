using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.AsyncNotWrappedIEnumerable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Async.NotWrappedIEnumerable;

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
    // The injection of the simple classes is wrapped in a IEnumerable<T>.
    // However, it is still not wrapped in manner of asynchronous injection.
    // Same follows for all other iterables (IReadOnlyList<T>, ICollection<T>, IList<T>, IReadOnlyCollection<T>, etc.) except for IAsyncEnumerable<T>.
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
        await using var container = Container.DIE_CreateContainer();
        // Notice that the Create-function returns a ValueTask<Parent> and not a Parent.
        // This is due to the necessary await in the Create-function, because the simple classes are initialized asynchronously but injected synchronously (not asynchronously wrapped).
        var parent = await container.Create();
        foreach (var dependency in parent.Dependencies)
        {
            Console.WriteLine($"Initialized: {dependency.Initialized}"); // Initialized: True
        }
    }
}