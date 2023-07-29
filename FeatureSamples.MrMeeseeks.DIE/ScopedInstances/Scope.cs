using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopedInstancesScope)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.ScopedInstances.Scope;

// The name of these simple classes indicates their lifetime scope. That means, how broad they will be shared.
internal class ConcreteClassPerScope { }
internal class ConcreteClassPerTransientScope { }

// The scope root has one dependency per kind
internal class ScopeRoot
{
    internal ScopeRoot(
        ConcreteClassPerScope concreteClassPerScope,
        ConcreteClassPerTransientScope concreteClassPerTransientScope)
    {
        ConcreteClassPerScope = concreteClassPerScope;
        ConcreteClassPerTransientScope = concreteClassPerTransientScope;
    }

    internal ConcreteClassPerScope ConcreteClassPerScope { get; }
    internal ConcreteClassPerTransientScope ConcreteClassPerTransientScope { get; }
}

// The transient scope root has one dependency per kind as well and an own child scope
internal class TransientScopeRoot
{
    
    internal TransientScopeRoot(
        ConcreteClassPerScope concreteClassPerScope,
        ConcreteClassPerTransientScope concreteClassPerTransientScope,
        ScopeRoot scopeRoot)
    {
        ConcreteClassPerScope = concreteClassPerScope;
        ConcreteClassPerTransientScope = concreteClassPerTransientScope;
        ScopeRoot = scopeRoot;
    }

    internal ConcreteClassPerScope ConcreteClassPerScope { get; }
    internal ConcreteClassPerTransientScope ConcreteClassPerTransientScope { get; }
    public ScopeRoot ScopeRoot { get; }
}

// The Parent class is no (transient) scope root, but it has a dependency of each kind (including the scope roots)
// Therefore, it will be created from within the container itself.
// For scoped instances, the container also works as the topmost (transient) scope.
internal class Parent
{
    internal Parent(
        ConcreteClassPerScope concreteClassPerScope,
        ConcreteClassPerTransientScope concreteClassPerTransientScope,
        ScopeRoot scopeRoot, 
        TransientScopeRoot transientScopeRoot)
    {
        ConcreteClassPerScope = concreteClassPerScope;
        ConcreteClassPerTransientScope = concreteClassPerTransientScope;
        ScopeRoot = scopeRoot;
        TransientScopeRoot = transientScopeRoot;
    }

    public ConcreteClassPerScope ConcreteClassPerScope { get; }
    public ConcreteClassPerTransientScope ConcreteClassPerTransientScope { get; }
    internal ScopeRoot ScopeRoot { get; }
    internal TransientScopeRoot TransientScopeRoot { get; }
}


[ImplementationAggregation(typeof(Parent), typeof(ScopeRoot), typeof(TransientScopeRoot), typeof(ConcreteClassPerScope), typeof(ConcreteClassPerTransientScope))]
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
// Register ConcreteClassPerScope as a scoped instance.
// That means, that it will be created once per scope and will be shared within that scope.
[ScopeInstanceImplementationAggregation(typeof(ConcreteClassPerScope))]
// Register ConcreteClassPerTransientScope as a transient scoped instance.
// That means, that it will be created once per transient scope and will be shared within that transient scope and its ordinary child scopes transitively.
[TransientScopeInstanceImplementationAggregation(typeof(ConcreteClassPerTransientScope))]
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
        
        // Because a ConcreteClassPerScope is shared per scope, the container's instance is unequal to the other scope's instances
        Console.WriteLine(parent.ConcreteClassPerScope == parent.ScopeRoot.ConcreteClassPerScope); // False
        Console.WriteLine(parent.ConcreteClassPerScope == parent.TransientScopeRoot.ConcreteClassPerScope); // False
        Console.WriteLine(parent.ConcreteClassPerScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerScope); // False
        
        // Same follows for the scope's instance
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerScope == parent.TransientScopeRoot.ConcreteClassPerScope); // False
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerScope); // False
        
        // And the transient scope's instance
        Console.WriteLine(parent.TransientScopeRoot.ConcreteClassPerScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerScope); // False
        
        // Because a ConcreteClassPerTransientScope is shared per transient scope, the container's instance is equal to the instance of its child scope
        // But not equal to the instances of the container's transient scope
        Console.WriteLine(parent.ConcreteClassPerTransientScope == parent.ScopeRoot.ConcreteClassPerTransientScope); // True
        Console.WriteLine(parent.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ConcreteClassPerTransientScope); // False
        Console.WriteLine(parent.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerTransientScope); // False
        
        // Consequently, the scope's instance is not equal to the transient scope's instance
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ConcreteClassPerTransientScope); // False
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerTransientScope); // False
        
        // But the transient scope's instance is equal to the instance of its child scope
        Console.WriteLine(parent.TransientScopeRoot.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerTransientScope); // True
    }
}