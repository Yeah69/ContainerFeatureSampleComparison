using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ScopesDefaultFragment)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Scopes.DefaultFragment;

internal interface IInterface {}

internal class ConcreteClassContainer : IInterface { }
internal class ConcreteClassScopeDefault : IInterface { }
internal class ConcreteClassScopeA : IInterface { }
internal class ConcreteClassScopeB : IInterface { }
internal class ConcreteClassTransientScopeDefault : IInterface { }
internal class ConcreteClassTransientScopeA : IInterface { }
internal class ConcreteClassTransientScopeB : IInterface { }


internal class ScopeRootDefault
{
    internal ScopeRootDefault(IInterface dependency) => Dependency = dependency;
    internal IInterface Dependency { get; }
}


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

internal class TransientScopeRootDefault
{
    internal TransientScopeRootDefault(IInterface dependency) => Dependency = dependency;
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
        ScopeRootDefault scopeRootDefault,
        ScopeRootA scopeRootA,
        ScopeRootB scopeRootB,
        TransientScopeRootDefault transientScopeRootDefault,
        TransientScopeRootA transientScopeRootA,
        TransientScopeRootB transientScopeRootB)
    {
        Dependency = dependency;
        ScopeRootDefault = scopeRootDefault;
        ScopeRootA = scopeRootA;
        ScopeRootB = scopeRootB;
        TransientScopeRootDefault = transientScopeRootDefault;
        TransientScopeRootA = transientScopeRootA;
        TransientScopeRootB = transientScopeRootB;
    }

    public IInterface Dependency { get; }
    public ScopeRootDefault ScopeRootDefault { get; }
    public ScopeRootA ScopeRootA { get; }
    public ScopeRootB ScopeRootB { get; }
    public TransientScopeRootDefault TransientScopeRootDefault { get; }
    public TransientScopeRootA TransientScopeRootA { get; }
    public TransientScopeRootB TransientScopeRootB { get; }
}

[ImplementationAggregation(typeof(Parent), typeof(ScopeRootDefault), typeof(ScopeRootA), typeof(ScopeRootB), typeof(TransientScopeRootDefault), typeof(TransientScopeRootA), typeof(TransientScopeRootB), typeof(ConcreteClassContainer), typeof(ConcreteClassScopeDefault), typeof(ConcreteClassScopeA), typeof(ConcreteClassScopeB), typeof(ConcreteClassTransientScopeDefault), typeof(ConcreteClassTransientScopeA), typeof(ConcreteClassTransientScopeB))]
[ScopeRootImplementationAggregation(typeof(ScopeRootDefault), typeof(ScopeRootA), typeof(ScopeRootB))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRootDefault), typeof(TransientScopeRootA), typeof(TransientScopeRootB))]
[ImplementationChoice(typeof(IInterface), typeof(ConcreteClassContainer))]
[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
    
    // The default scope doesn't need to be assigned to a scope root type
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScopeDefault))]
    private sealed partial class DIE_DefaultScope {}
    
    [CustomScopeForRootTypes(typeof(ScopeRootA))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScopeA))]
    private sealed partial class DIE_ScopeA {}
    
    [CustomScopeForRootTypes(typeof(ScopeRootB))]
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassScopeB))]
    private sealed partial class DIE_ScopeB {}
    
    // The default transient scope doesn't need to be assigned to a transient scope root type
    [ImplementationChoice(typeof(IInterface), typeof(ConcreteClassTransientScopeDefault))]
    private sealed partial class DIE_DefaultTransientScope {}
    
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
        Console.WriteLine($"DefaultScope: {parent.ScopeRootDefault.Dependency.GetType().Name}"); // DefaultScope: ConcreteClassScopeDefault
        Console.WriteLine($"ScopeA: {parent.ScopeRootA.Dependency.GetType().Name}"); // ScopeA: ConcreteClassScopeA
        Console.WriteLine($"ScopeB: {parent.ScopeRootB.Dependency.GetType().Name}"); // ScopeB: ConcreteClassScopeB
        Console.WriteLine($"DefaultTransientScope: {parent.TransientScopeRootDefault.Dependency.GetType().Name}"); // DefaultTransientScope: ConcreteClassTransientScopeDefault
        Console.WriteLine($"TransientScopeA: {parent.TransientScopeRootA.Dependency.GetType().Name}"); // TransientScopeA: ConcreteClassTransientScopeA
        Console.WriteLine($"TransientScopeB: {parent.TransientScopeRootB.Dependency.GetType().Name}"); // TransientScopeB: ConcreteClassTransientScopeB
    }
}