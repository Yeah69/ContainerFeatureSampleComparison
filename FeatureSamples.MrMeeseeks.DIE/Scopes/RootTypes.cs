using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesRootTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.RootTypes;

internal class ConcreteClass : IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

internal class ScopeRoot
{
    internal ScopeRoot(ConcreteClass dependency) => Dependency = dependency;
    internal ConcreteClass Dependency { get; }
}

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
    internal void CleanUp() => _transientScopeDisposal.Dispose();
}

internal class Parent
{
    internal Parent(
        // Scope roots can be injected like any other dependency. The container will wrap a scope around such a scope root dependency.
        ScopeRoot scopeRoot, 
        // The same applies to transient scope roots
        TransientScopeRoot transientScopeRoot,
        // This transient scope root won't be disposed eagerly but will be disposed along the container's disposal
        TransientScopeRoot disposedByContainer)
    {
        ScopeRoot = scopeRoot;
        TransientScopeRoot = transientScopeRoot;
        DisposedByContainer = disposedByContainer;
    }

    internal ScopeRoot ScopeRoot { get; }
    internal TransientScopeRoot TransientScopeRoot { get; }
    public TransientScopeRoot DisposedByContainer { get; }
}


[ImplementationAggregation(typeof(Parent), typeof(ScopeRoot), typeof(TransientScopeRoot), typeof(ConcreteClass))]
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        // Do something with the parent and/or its dependencies
        Console.WriteLine($"Disposed: {parent.ScopeRoot.Dependency.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {parent.TransientScopeRoot.Dependency.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {parent.DisposedByContainer.Dependency.Disposed}"); // Disposed: False
        parent.TransientScopeRoot.CleanUp();
        Console.WriteLine($"Disposed: {parent.ScopeRoot.Dependency.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {parent.TransientScopeRoot.Dependency.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {parent.DisposedByContainer.Dependency.Disposed}"); // Disposed: False
        container.Dispose();
        Console.WriteLine($"Disposed: {parent.ScopeRoot.Dependency.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {parent.TransientScopeRoot.Dependency.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {parent.DisposedByContainer.Dependency.Disposed}"); // Disposed: True
    }
}