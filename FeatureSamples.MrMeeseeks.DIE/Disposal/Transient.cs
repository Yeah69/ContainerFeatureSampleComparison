using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.Transient;

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
[SyncTransientImplementationAggregation(typeof(ConcreteClassSync))]
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
        container.Dispose();
        Console.WriteLine($"Disposed: {concreteClassSync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
    }
}