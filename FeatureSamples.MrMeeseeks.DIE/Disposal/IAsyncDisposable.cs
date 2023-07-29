using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalIAsyncDisposable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.IAsyncDisposable;

// This simple class is asynchronously disposable
internal class ConcreteClass : System.IAsyncDisposable
{
    internal bool Disposed { get; private set; }
    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}

[ImplementationAggregation(typeof(ConcreteClass))]
// The container manages the disposal of IAsyncDisposable instances per default.
// Therefore, there is no need to configure the container to do so.
[CreateFunction(typeof(ConcreteClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        var container = Container.DIE_CreateContainer();
        var concreteClass = container.Create();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        // As soon as the container is disposed, all its managed disposable dependencies are disposed as well.
        // Notice that the container has only the asynchronous DisposeAsync method, but no synchronous Dispose method.
        // If the container manages at least one IAsyncDisposable dependency, it will only have the asynchronous DisposeAsync method.
        await container.DisposeAsync();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}