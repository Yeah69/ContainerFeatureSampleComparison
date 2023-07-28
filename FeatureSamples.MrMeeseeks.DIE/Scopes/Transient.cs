using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.Transient;

// Simple class that will be implicitly inside a scope, because it'll be injected into a scope root
internal class ConcreteClass : IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

// A transient scope root. Anytime that this class is created or injected, a new transient scope is created which will take over creation of this transient scope root.
internal class TransientScopeRoot
{
    private readonly IDisposable _transientScopeDisposal;

    internal TransientScopeRoot(
        ConcreteClass dependency, 
        // A transient scope root can get an IDisposable instance which can be used to trigger the disposal of the transient scope eagerly
        IDisposable transientScopeDisposal)
    {
        _transientScopeDisposal = transientScopeDisposal;
        Dependency = dependency;
    }

    internal ConcreteClass Dependency { get; }
    // A transient scope root can decide on its own when to dispose the transient scope and all its managed disposable dependencies.
    internal void CleanUp() => _transientScopeDisposal.Dispose();
}

[ImplementationAggregation(typeof(TransientScopeRoot), typeof(ConcreteClass))]
// Following configuration makes the TransientScopeRoot a transient scope root.
// The transient scope that creates the transient scope root will be generated into the container.
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
[CreateFunction(typeof(TransientScopeRoot), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        // Creating a transient scope root
        var transientScopeRoot = container.Create();
        Console.WriteLine($"Disposed: {transientScopeRoot.Dependency.Disposed}"); // Disposed: False
        // When a transient scope root triggers the disposal of its transient scope, the transient scope and all its managed disposable dependencies will be disposed.
        // This includes also all ordinary child scopes that were created from within the transient scope.
        // Disposing the container will dispose all transient scopes that weren't disposed yet.
        transientScopeRoot.CleanUp();
        Console.WriteLine($"Disposed: {transientScopeRoot.Dependency.Disposed}"); // Disposed: True
    }
}