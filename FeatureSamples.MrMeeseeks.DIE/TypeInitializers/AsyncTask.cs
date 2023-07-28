using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TypeInitializersAsyncTask)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.TypeInitializers.AsyncTask;

internal class ConcreteClassA
{
    internal bool Initialized { get; private set; }
    // This method is called by the container after the instance is created and before it is further synchronously injected (injections which aren't wrapped in ValueTask<T>/Task<T>).
    // With asynchronous injections (injection wrapped into either ValueTask<T> or Task<T>), it may be injected eagerly, then the consuming component may decide to await, block or fire/forget.
    // The container is guaranteed to have no blocking calls.
    internal async Task Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

// You can also use an interface as a marker for type initialization
internal interface IAsyncTypeInitializer
{
    Task Initialize();
}

internal class ConcreteClassB : IAsyncTypeInitializer
{
    internal bool Initialized { get; private set; }
    // With a marker interface you have the option to implement the interface explicitly
    // That way the method isn't directly visible on references of the implementation type without casting to the interface
    async Task IAsyncTypeInitializer.Initialize()
    {
        await Task.Yield();
        Initialized = true;
    }
}

internal class Parent
{
    internal Parent(ValueTask<ConcreteClassB> concreteClassB) => ConcreteClassB = concreteClassB;
    internal ValueTask<ConcreteClassB> ConcreteClassB { get; }
}

[ImplementationAggregation(typeof(ConcreteClassA), typeof(ConcreteClassB), typeof(Parent))]
// Register the type initializer methods
[Initializer(typeof(ConcreteClassA), nameof(ConcreteClassA.Initialize))]
[Initializer(typeof(IAsyncTypeInitializer), nameof(IAsyncTypeInitializer.Initialize))]

[CreateFunction(typeof(ConcreteClassA), "CreateA")]
[CreateFunction(typeof(Parent), "CreateB")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static async Task Use()
    {
        await using var container = Container.DIE_CreateContainer();
        // Notice that the Create-function is now async and returns a ValueTask<ConcreteClassA> instead of ConcreteClassA.
        // In order to fulfill the aforementioned guarantees, the container has to await the type initializer before returning the instance.
        // An Create-function with "async void" modifiers would force fire-and-forget semantics, therefore the container wraps the returned type into a ValueTask<T> automatically.
        // It upon the user to decide whether to await the returned ValueTask<T> or not.
        var concreteClassA = await container.CreateA();
        Console.WriteLine($"Initialized: {concreteClassA.Initialized}"); // Initialized: True
        // Notice that the Create-function for the Parent type is still synchronous.
        // That is because its ConcreteClassB-injection is wrapped in a ValueTask<T> and therefore the container doesn't need to generate any awaits.
        var parent = container.CreateB();
        Console.WriteLine($"Initialized: {(await parent.ConcreteClassB).Initialized}"); // Initialized: True
    }
}