using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.MiscInitializedInstance)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Misc.InitializedInstance;

internal class ConcreteClass
{
}

internal class Parent
{
    internal Parent(ConcreteClass dependency) => Dependency = dependency;
    public ConcreteClass Dependency { get; set; }
}

internal class ScopeRoot
{
    internal ScopeRoot(ConcreteClass dependency) => Dependency = dependency;
    internal ConcreteClass Dependency { get; }
}

[ImplementationAggregation(typeof(ConcreteClass), typeof(Parent), typeof(ScopeRoot))]
[InitializedInstances(typeof(ConcreteClass))]
[ScopeRootImplementationAggregation(typeof(ScopeRoot))]
[CreateFunction(typeof(ConcreteClass), "Create")]
[CreateFunction(typeof(ScopeRoot), "CreateScope")]
internal partial class Container
{
    private Container() {}
    
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
        // Or initialized instances are implicitly and effectively much like scoped instances
        Console.WriteLine(concreteClassSA != concreteClassSB); // True
    }
}