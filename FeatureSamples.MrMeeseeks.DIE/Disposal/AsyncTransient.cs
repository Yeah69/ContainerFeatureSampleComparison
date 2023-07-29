using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalAsyncTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.AsyncTransient;

// Following two simple classes are disposable
internal class ConcreteClassAsync : System.IAsyncDisposable
{
    internal bool Disposed { get; private set; }
    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}

internal class ConcreteClass : System.IAsyncDisposable
{
    internal bool Disposed { get; private set; }
    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}

[ImplementationAggregation(typeof(ConcreteClassAsync), typeof(ConcreteClass))]
// In order to prevent the container from managing the disposal of IAsyncDisposable instances, implementation types can be declared as transient.
// Following attribute corresponds only to IAsyncDisposable instances. That means if a type would implement both IDisposable and IAsyncDisposable, the container would still manage the disposal but only with IDisposable.Dispose().
[AsyncTransientImplementationAggregation(typeof(ConcreteClassAsync))]
// Following attribute corresponds to both IDisposable and IAsyncDisposable instances. That means independent of which disposable interfaces are implemented by the type, the container won't manage the disposal for it.
[TransientImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(ConcreteClassAsync), "AsyncCreate")]
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
        var concreteClassAsync = container.AsyncCreate();
        var concreteClass = container.Create();
        Console.WriteLine($"Disposed: {concreteClassAsync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        // Disposing the container won't dispose the disposable dependencies.
        await container.DisposeAsync();
        Console.WriteLine($"Disposed: {concreteClassAsync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        await concreteClassAsync.DisposeAsync();
        await concreteClass.DisposeAsync();
        Console.WriteLine($"Disposed: {concreteClassAsync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}