using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesReconfiguration)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.Reconfiguration;

internal interface IInterface {}

// The names of following simple classes indicate in which kind of scope they'll be created.
internal class ConcreteClassContainer : IInterface { }
internal class ConcreteClassScope : IInterface { }
internal class ConcreteClassTransientScope : IInterface { }

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
    
    // Per default scopes inherit all attributed configurations from the container.
    // Optionally, they can be reconfigured.
    // This is done by creating a partial class which name starts with "DIE_Scope" and has following attribute which specifies the scope root type for which it is applied.
    [CustomScopeForRootTypes(typeof(ScopeRoot))]
    // Here we reconfigure the implementation choice for IInterface to ConcreteClassScope
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScope))]
    private sealed partial class DIE_Scope {}
    
    // Reconfiguration for transient scopes works the same way but the partial class name must start with "DIE_TransientScope".
    [CustomScopeForRootTypes(typeof(TransientScopeRoot))]
    // Here we reconfigure the implementation choice for IInterface to ConcreteClassTransientScope
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassTransientScope))]
    private sealed partial class DIE_TransientScope {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        // Each scope gets its configured implementation choice for IInterface injected.
        Console.WriteLine($"Container: {parent.Dependency.GetType().Name}"); // Container: ConcreteClassContainer
        Console.WriteLine($"Scope: {parent.ScopeRoot.Dependency.GetType().Name}"); // , Scope: ConcreteClassScope
        Console.WriteLine($"TransientScope: {parent.TransientScopeRoot.Dependency.GetType().Name}"); // TransientScope: ConcreteClassTransientScope
    }
}