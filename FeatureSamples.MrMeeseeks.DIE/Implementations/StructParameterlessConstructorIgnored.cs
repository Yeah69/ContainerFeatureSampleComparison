using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.ImplementationsStructParameterlessConstructorIgnored)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Implementations.StructParameterlessConstructorIgnored;

// This struct has an explicit constructor in addition to the parameterless one this makes two constructors
// That means that it is technically ambiguous which constructor to choose
internal struct Struct
{
    internal Struct(int value) => Value = value; 
    internal int Value { get; }
}

// Configure no constructor choice
// The container should implicitly ignore the parameterless constructor
// Therefore, it should choose the single explicit constructor
[ImplementationAggregation(typeof(Struct))]
[CreateFunction(typeof(Struct), "Create")]
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
        var value = container.Create();
        Console.WriteLine($"The value is {value.Value}"); // The value is 69
    }
}