using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesSimple)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.Simple;

// Simple class that will be implicitly inside a scope, because it'll be injected into a scope root
internal class ConcreteClass : IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

// A scope root. Anytime that this class is created or injected, a new scope is created which will take over creation of this scope root.
internal class ScopeRoot
{
    internal ScopeRoot(ConcreteClass dependency) => Dependency = dependency;
    internal ConcreteClass Dependency { get; }
}

[ImplementationAggregation(typeof(ScopeRoot), typeof(ConcreteClass))]
// Following configuration makes the ScopeRoot a scope root.
// The scope that creates the scope root will be generated into the container.
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[CreateFunction(typeof(ScopeRoot), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        // Resolving a scope root
        var scopeRoot = container.Create();
        Console.WriteLine($"Disposed: {scopeRoot.Dependency.Disposed}"); // Disposed: False
        // Disposing the container will dispose all its scopes which in turn will dispose all their managed disposable dependencies.
        container.Dispose();
        Console.WriteLine($"Disposed: {scopeRoot.Dependency.Disposed}"); // Disposed: True
    }
}