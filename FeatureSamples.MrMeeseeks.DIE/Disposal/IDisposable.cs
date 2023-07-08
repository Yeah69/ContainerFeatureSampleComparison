using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalIDisposable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.IDisposable;

internal class ConcreteClass : System.IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

[ImplementationAggregation(typeof(ConcreteClass))]
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
        container.Dispose();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}