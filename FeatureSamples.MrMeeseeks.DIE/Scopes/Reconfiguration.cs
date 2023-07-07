using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesReconfiguration)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.Reconfiguration;

internal interface IInterface {}

internal class ConcreteClassContainer : IInterface
{
}

internal class ConcreteClassScope : IInterface
{
}

internal class ConcreteClassTransientScope : IInterface
{
}

internal class ScopeRoot
{
    internal ScopeRoot(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}

internal class TransientScopeRoot
{
    internal TransientScopeRoot(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}

internal class Parent
{
    internal Parent(
        IInterface dependency,
        ScopeRoot scopeRoot, 
        TransientScopeRoot transientScopeRoot)
    {
        Dependency = dependency;
        ScopeRoot = scopeRoot;
        TransientScopeRoot = transientScopeRoot;
    }

    public IInterface Dependency { get; }
    internal ScopeRoot ScopeRoot { get; }
    internal TransientScopeRoot TransientScopeRoot { get; }
}


[ImplementationAggregation(typeof(Parent), typeof(ScopeRoot), typeof(TransientScopeRoot), typeof(ConcreteClassContainer), typeof(ConcreteClassScope), typeof(ConcreteClassTransientScope))]
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
[ImplementationChoice(typeof(IInterface), typeof(ConcreteClassContainer))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
    
    [CustomScopeForRootTypes(typeof(ScopeRoot))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScope))]
    private sealed partial class DIE_Scope {}
    
    [CustomScopeForRootTypes(typeof(TransientScopeRoot))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassTransientScope))]
    private sealed partial class DIE_TransientScope {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        Console.WriteLine($"Container: {parent.Dependency.GetType().Name}"); // Container: ConcreteClassContainer
        Console.WriteLine($"Scope: {parent.ScopeRoot.Dependency.GetType().Name}"); // , Scope: ConcreteClassScope
        Console.WriteLine($"TransientScope: {parent.TransientScopeRoot.Dependency.GetType().Name}"); // TransientScope: ConcreteClassTransientScope
    }
}