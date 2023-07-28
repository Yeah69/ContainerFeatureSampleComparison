using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TypeInitializersSync)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.TypeInitializers.Sync;

internal class ConcreteClassA
{
    internal bool Initialized { get; private set; }
    // This method is called by the container after the instance is created and before it is further injected
    internal void Initialize() => Initialized = true;
}

// You can also use an interface as a marker for type initialization
internal interface ITypeInitializer
{
    void Initialize();
}

internal class ConcreteClassB : ITypeInitializer
{
    internal bool Initialized { get; private set; }
    // With a marker interface you have the option to implement the interface explicitly
    // That way the method isn't directly visible on references of the implementation type without casting to the interface
    void ITypeInitializer.Initialize() => Initialized = true;
}

[ImplementationAggregation(typeof(ConcreteClassA), typeof(ConcreteClassB))]
// Register the type initializer methods
[Initializer(typeof(ConcreteClassA), nameof(ConcreteClassA.Initialize))]
[Initializer(typeof(ITypeInitializer), nameof(ITypeInitializer.Initialize))]

[CreateFunction(typeof(ConcreteClassA), "CreateA")]
[CreateFunction(typeof(ConcreteClassB), "CreateB")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClassA = container.CreateA();
        Console.WriteLine($"Initialized: {concreteClassA.Initialized}"); // Initialized: True
        var concreteClassB = container.CreateB();
        Console.WriteLine($"Initialized: {concreteClassB.Initialized}"); // Initialized: True
    }
}