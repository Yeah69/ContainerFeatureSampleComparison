using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.Transient;

// Following two simple classes are disposable
internal class ConcreteClassSync : System.IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

internal class ConcreteClass : System.IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

[ImplementationAggregation(typeof(ConcreteClassSync), typeof(ConcreteClass))]
// In order to prevent the container from managing the disposal of IDisposable instances, implementation types can be declared as transient.
// Following attribute corresponds only to IDisposable instances. That means if a type would implement both IDisposable and IAsyncDisposable, the container would still manage the disposal but only with IAsyncDisposable.DisposeAsync().
[SyncTransientImplementationAggregation(typeof(ConcreteClassSync))]
// Following attribute corresponds to both IDisposable and IAsyncDisposable instances. That means independent of which disposable interfaces are implemented by the type, the container won't manage the disposal for it.
[TransientImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(ConcreteClassSync), "CreateSync")]
[CreateFunction(typeof(ConcreteClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var concreteClassSync = container.CreateSync();
        var concreteClass = container.Create();
        Console.WriteLine($"Disposed: {concreteClassSync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        // Disposing the container won't dispose the disposable dependencies.
        container.Dispose();
        Console.WriteLine($"Disposed: {concreteClassSync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        concreteClassSync.Dispose();
        concreteClass.Dispose();
        Console.WriteLine($"Disposed: {concreteClassSync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}