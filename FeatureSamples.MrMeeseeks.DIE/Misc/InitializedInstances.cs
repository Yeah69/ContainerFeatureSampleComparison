using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.MiscInitializedInstance)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Misc.InitializedInstance;

// This simple class will be used as an initialized instance in the container and the scope
internal class ConcreteClass { }

internal class ScopeRoot
{
    internal ScopeRoot(ConcreteClass dependency) => Dependency = dependency;
    internal ConcreteClass Dependency { get; }
}

[ImplementationAggregation(typeof(ConcreteClass), typeof(ScopeRoot))]
// Initialized instances can be defined for a container.
// They will be created as soon as the container is created and then used for injections to their type.
[InitializedInstances(typeof(ConcreteClass))]
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[CreateFunction(typeof(ConcreteClass), "Create")]
[CreateFunction(typeof(ScopeRoot), "CreateScope")]
internal partial class Container
{
    private Container() {}
    
    // Initialized instances can also be defined for a scope.
    // They will be created as soon as the scope is created and then used for injections to their type.
    [InitializedInstances(typeof(ConcreteClass))]
    private partial class DIE_DefaultScope {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClassA = container.Create();
        var concreteClassB = container.Create();
        // Initialized instances are implicitly and effectively much like singletons/"container instances"
        Console.WriteLine(concreteClassA == concreteClassB); // True
        
        var concreteClassSA = container.CreateScope().Dependency;
        var concreteClassSB = container.CreateScope().Dependency;
        // Or initialized instances are implicitly and effectively much like scoped instances, if used in a scope.
        Console.WriteLine(concreteClassSA != concreteClassSB); // True
    }
}