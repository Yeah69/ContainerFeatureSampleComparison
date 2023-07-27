using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.TuplesTuple)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Tuples.Tuple;

internal class ConcreteClass {}

internal interface IInterface {}

internal class Implementation : IInterface {}

[ImplementationAggregation(typeof(ConcreteClass), typeof(Implementation))]
// Return a Tuple<ConcreteClass, IInterface, int> without explicitly registering it
[CreateFunction(typeof(Tuple<ConcreteClass, IInterface, int>), "Create")]
internal partial class Container
{
    private Container() {}
    
    private int DIE_Factory => 42;
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var (concreteClass, implementation, number) = container.Create();
        Console.WriteLine(concreteClass.GetType().Name); // ConcreteClass
        Console.WriteLine(implementation.GetType().Name); // Implementation
        Console.WriteLine(number.GetType().Name); // Int32
        Console.WriteLine(number); // 42
    }
}