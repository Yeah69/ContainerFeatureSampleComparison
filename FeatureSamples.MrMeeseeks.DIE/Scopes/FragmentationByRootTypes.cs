using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesFragmentationByRootTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.FragmentationByRootTypes;

internal interface IInterface {}

internal class ConcreteClassContainer : IInterface { }
internal class ConcreteClassScopeA : IInterface { }
internal class ConcreteClassScopeB : IInterface { }
internal class ConcreteClassTransientScopeA : IInterface { }
internal class ConcreteClassTransientScopeB : IInterface { }


internal class ScopeRootA
{
    internal ScopeRootA(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}

internal class ScopeRootB
{
    internal ScopeRootB(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}

internal class TransientScopeRootA
{
    internal TransientScopeRootA(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}

internal class TransientScopeRootB
{
    internal TransientScopeRootB(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}

internal class Parent
{
    internal Parent(
        IInterface dependency,
        ScopeRootA scopeRootA,
        ScopeRootB scopeRootB,
        TransientScopeRootA transientScopeRootA,
        TransientScopeRootB transientScopeRootB)
    {
        Dependency = dependency;
        ScopeRootA = scopeRootA;
        ScopeRootB = scopeRootB;
        TransientScopeRootA = transientScopeRootA;
        TransientScopeRootB = transientScopeRootB;
    }

    public IInterface Dependency { get; }
    public ScopeRootA ScopeRootA { get; }
    public ScopeRootB ScopeRootB { get; }
    public TransientScopeRootA TransientScopeRootA { get; }
    public TransientScopeRootB TransientScopeRootB { get; }
}

[ImplementationAggregation(typeof(Parent), typeof(ScopeRootA), typeof(ScopeRootB), typeof(TransientScopeRootA), typeof(TransientScopeRootB), typeof(ConcreteClassContainer), typeof(ConcreteClassScopeA), typeof(ConcreteClassScopeB), typeof(ConcreteClassTransientScopeA), typeof(ConcreteClassTransientScopeB))]
[ScopeRootImplementationAggregation(typeof(ScopeRootA), typeof(ScopeRootB))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRootA), typeof(TransientScopeRootB))]
[ImplementationChoice(typeof(IInterface), typeof(ConcreteClassContainer))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
    
    // Each (transient) scope root can also get its own dedicated (transient) scope configuration.
    [CustomScopeForRootTypes(typeof(ScopeRootA))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScopeA))]
    private sealed partial class DIE_ScopeA {}
    
    [CustomScopeForRootTypes(typeof(ScopeRootB))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScopeB))]
    private sealed partial class DIE_ScopeB {}
    
    [CustomScopeForRootTypes(typeof(TransientScopeRootA))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassTransientScopeA))]
    private sealed partial class DIE_TransientScopeA {}
    
    [CustomScopeForRootTypes(typeof(TransientScopeRootB))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassTransientScopeB))]
    private sealed partial class DIE_TransientScopeB {}
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        // Each scope gets its configured implementation choice for IInterface injected.
        Console.WriteLine($"Container: {parent.Dependency.GetType().Name}"); // Container: ConcreteClassContainer
        Console.WriteLine($"ScopeA: {parent.ScopeRootA.Dependency.GetType().Name}"); // ScopeA: ConcreteClassScopeA
        Console.WriteLine($"ScopeB: {parent.ScopeRootB.Dependency.GetType().Name}"); // ScopeB: ConcreteClassScopeB
        Console.WriteLine($"TransientScopeA: {parent.TransientScopeRootA.Dependency.GetType().Name}"); // TransientScopeA: ConcreteClassTransientScopeA
        Console.WriteLine($"TransientScopeB: {parent.TransientScopeRootB.Dependency.GetType().Name}"); // TransientScopeB: ConcreteClassTransientScopeB
    }
}