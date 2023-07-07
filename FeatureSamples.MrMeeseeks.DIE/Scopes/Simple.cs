using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesSimple)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.Simple;

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

[ImplementationAggregation(typeof(ScopeRoot), typeof(ConcreteClass))]
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
        var scopeRoot = container.Create();
        // Do something with scope root and/or its dependency
        Console.WriteLine($"Disposed: {scopeRoot.Dependency.Disposed}"); // Disposed: False
        container.Dispose();
        Console.WriteLine($"Disposed: {scopeRoot.Dependency.Disposed}"); // Disposed: True
    }
}