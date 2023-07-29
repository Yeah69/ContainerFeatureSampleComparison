using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalIDisposable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.IDisposable;

// This simple class is disposable
internal class ConcreteClass : System.IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

[ImplementationAggregation(typeof(ConcreteClass))]
// The container manages the disposal of IDisposable instances per default.
// Therefore, there is no need to configure the container to do so.
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
        var concreteClass = container.Create();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        // As soon as the container is disposed, all its managed disposable dependencies are disposed as well.
        container.Dispose();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}