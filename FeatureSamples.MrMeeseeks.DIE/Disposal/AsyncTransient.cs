using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalAsyncTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.AsyncTransient;

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
[AsyncTransientImplementationAggregation(typeof(ConcreteClassAsync))]
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
        await container.DisposeAsync();
        Console.WriteLine($"Disposed: {concreteClassAsync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
    }
}