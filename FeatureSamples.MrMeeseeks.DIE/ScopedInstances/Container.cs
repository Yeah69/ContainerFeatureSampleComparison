using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopedInstancesContainer)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.ScopedInstances.Container;

internal class ConcreteClassPerContainer
{
}

internal class ScopeRoot
{
    internal ScopeRoot(ConcreteClassPerContainer concreteClassPerContainer) =>
        ConcreteClassPerContainer = concreteClassPerContainer;
    
    internal ConcreteClassPerContainer ConcreteClassPerContainer { get; }
}

internal class TransientScopeRoot
{
    internal TransientScopeRoot(
        ConcreteClassPerContainer concreteClassPerContainer,
        ScopeRoot scopeRoot)
    {
        ConcreteClassPerContainer = concreteClassPerContainer;
        ScopeRoot = scopeRoot;
    }

    internal ConcreteClassPerContainer ConcreteClassPerContainer { get; }
    public ScopeRoot ScopeRoot { get; }
}

internal class Parent
{
    internal Parent(
        ConcreteClassPerContainer concreteClassPerContainer,
        ScopeRoot scopeRoot, 
        TransientScopeRoot transientScopeRoot)
    {
        ConcreteClassPerContainer = concreteClassPerContainer;
        ScopeRoot = scopeRoot;
        TransientScopeRoot = transientScopeRoot;
    }

    internal ConcreteClassPerContainer ConcreteClassPerContainer { get; }
    internal ScopeRoot ScopeRoot { get; }
    internal TransientScopeRoot TransientScopeRoot { get; }
}


[ImplementationAggregation(typeof(Parent), typeof(ScopeRoot), typeof(TransientScopeRoot), typeof(ConcreteClassPerContainer))]
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
[ContainerInstanceImplementationAggregation(typeof(ConcreteClassPerContainer))]
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
        
        Console.WriteLine(parent.ConcreteClassPerContainer == parent.ScopeRoot.ConcreteClassPerContainer); // True
        Console.WriteLine(parent.ConcreteClassPerContainer == parent.TransientScopeRoot.ConcreteClassPerContainer); // True
        Console.WriteLine(parent.ConcreteClassPerContainer == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerContainer); // True
        
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerContainer == parent.TransientScopeRoot.ConcreteClassPerContainer); // True
        Console.WriteLine(parent.ScopeRoot.ConcreteClassPerContainer == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerContainer); // True
        
        Console.WriteLine(parent.TransientScopeRoot.ConcreteClassPerContainer == parent.TransientScopeRoot.ScopeRoot.ConcreteClassPerContainer); // True
    }
}