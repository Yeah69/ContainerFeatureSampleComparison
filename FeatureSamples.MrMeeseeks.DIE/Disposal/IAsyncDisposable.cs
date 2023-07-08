using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalIAsyncDisposable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.IAsyncDisposable;

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
        await container.DisposeAsync();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}