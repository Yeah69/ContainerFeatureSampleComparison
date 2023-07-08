using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopedInstancesScope)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.ScopedInstances.Scope;

internal class ConcreteClassPerScope
{
}

internal class ConcreteClassPerTransientScope
{
}

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
[ScopeInstanceImplementationAggregation(typeof(ConcreteClassPerScope))]
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
        
        Console.WriteLine(parent.ConcreteClassPerScope == parent.ScopeRoot.ConcreteClassPerScope); // False
        Console.WriteLine(parent.ConcreteClassPerScope == parent.TransientScopeRoot.ConcreteClassPerScope); // False
        Console.WriteLine(parent.ConcreteClassPerScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerScope); // False
        
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerScope == parent.TransientScopeRoot.ConcreteClassPerScope); // False
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerScope); // False
        
        Console.WriteLine(parent.TransientScopeRoot.ConcreteClassPerScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerScope); // False
        
        Console.WriteLine(parent.ConcreteClassPerTransientScope == parent.ScopeRoot.ConcreteClassPerTransientScope); // True
        Console.WriteLine(parent.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ConcreteClassPerTransientScope); // False
        Console.WriteLine(parent.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerTransientScope); // False
        
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ConcreteClassPerTransientScope); // False
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerTransientScope); // False
        
        Console.WriteLine(parent.TransientScopeRoot.ConcreteClassPerTransientScope == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerTransientScope); // True
    }
}