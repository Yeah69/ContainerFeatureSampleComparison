using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.Transient;

internal class ConcreteClass : IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
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

[ImplementationAggregation(typeof(TransientScopeRoot), typeof(ConcreteClass))]
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
        var transientScopeRoot = container.Create();
        // Do something with transient scope root and/or its dependency
        Console.WriteLine($"Disposed: {transientScopeRoot.Dependency.Disposed}"); // Disposed: False
        transientScopeRoot.CleanUp();
        Console.WriteLine($"Disposed: {transientScopeRoot.Dependency.Disposed}"); // Disposed: True
    }
}