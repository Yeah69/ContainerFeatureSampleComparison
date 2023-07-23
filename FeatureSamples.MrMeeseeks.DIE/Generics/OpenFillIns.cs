using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.GenericsOpenFillIns)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Generics.OpenFillIns;

// A generic interface with only one generic parameter
internal interface IInterface<T>
{
}

// A generic class implementing the generic interface but with two generic parameters
internal class ConcreteClass<T, TA> : IInterface<T>
{
}

// Register the generic implementation
[ImplementationAggregation(typeof(ConcreteClass<,>))]
// Choose string as the default type for the second generic parameter
[GenericParameterChoice(typeof(ConcreteClass<,>), "TA", typeof(string))]
// Choose some other types for the second generic parameter
[GenericParameterSubstitutesChoice(typeof(ConcreteClass<,>), "TA", typeof(bool), typeof(double))]
// We'll test resolution of a single instance of the interface
[CreateFunction(typeof(IInterface<int>), "CreateSingular")]
// And resolution of multiple instances of the interface
[CreateFunction(typeof(IInterface<int>[]), "CreateMultiple")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        // The singular option will fill the open generic parameter with string
        var singular = container.CreateSingular();
        Console.WriteLine($"{singular.GetType().Name} {singular.GetType().GenericTypeArguments.First().Name} {singular.GetType().GenericTypeArguments.Skip(1).First().Name}"); // ConcreteClass`2 Int32 String
        // The multiple option will fill the open generic parameter with string, bool and double
        // That means the singular option is implicitly included in the multiple option
        var multiple = container.CreateMultiple();
        foreach (var instance in multiple)
        {
            Console.WriteLine($"{instance.GetType().Name} {instance.GetType().GenericTypeArguments.First().Name} {instance.GetType().GenericTypeArguments.Skip(1).First().Name}");
        }
    }
}