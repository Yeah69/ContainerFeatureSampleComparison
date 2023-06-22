using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsExplicitConstructorChoice)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.ExplicitConstructorChoice;

// This class has two constructors, one with an int parameter and one without
// That means that it is ambiguous which constructor to choose
internal class ConcreteClass
{
    internal ConcreteClass() => Value = 68;
    internal ConcreteClass(int value) => Value = value;
    
    internal int Value { get; }
}

[ImplementationAggregation(typeof(ConcreteClass))]
[CreateFunction(typeof(ConcreteClass), "Create")]

// Choose the constructor with the int parameter
[ConstructorChoice(typeof(ConcreteClass), typeof(int))]
internal partial class Container
{
    private Container() {}

    // Custom factory for the int parameter
    private int DIE_Factory => 69;
}

internal static class Usage
{
    internal static void Use()
    {
        var container = Container.DIE_CreateContainer();
        var concreteClass = container.Create();
        Console.WriteLine($"The value is {concreteClass.Value}"); // The value is 69
    }
}