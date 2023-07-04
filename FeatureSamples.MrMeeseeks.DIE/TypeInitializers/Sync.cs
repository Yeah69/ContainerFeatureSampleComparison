using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TypeInitializersSync)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.TypeInitializers.Sync;

internal class ConcreteClass
{
    internal bool Initialized { get; private set; }
    internal void Initialize() => Initialized = true;
}

[ImplementationAggregation(typeof(ConcreteClass))]
[Initializer(typeof(ConcreteClass), nameof(ConcreteClass.Initialize))]
[CreateFunction(typeof(ConcreteClass), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClass = container.Create();
        Console.WriteLine($"Initialized: {concreteClass.Initialized}"); // Initialized: True
    }
}