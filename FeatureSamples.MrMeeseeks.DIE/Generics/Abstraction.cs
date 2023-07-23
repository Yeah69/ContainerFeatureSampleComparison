using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.GenericsAbstraction)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Generics.Abstraction;

// Different types of generic abstractions
internal interface IInterface<T>
{
}

internal abstract class AbstractClass<T>
{
}

// An implementation of the generic abstractions
internal class ConcreteClass<T> : AbstractClass<T>, IInterface<T>
{
}

// Register the generic implementation
[ImplementationAggregation(typeof(ConcreteClass<>))]
// Define create functions for some closed versions of the generic abstraction types
[CreateFunction(typeof(IInterface<int>), "CreateIInterfaceOfInt")]
[CreateFunction(typeof(IInterface<string>), "CreateIInterfaceOfString")]
[CreateFunction(typeof(AbstractClass<int>), "CreateAbstractClassOfInt")]
[CreateFunction(typeof(AbstractClass<string>), "CreateAbstractClassOfString")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var iInterfaceClassOfInt = container.CreateIInterfaceOfInt();
        var iInterfaceOfString = container.CreateIInterfaceOfString();
        var abstractClassOfInt = container.CreateAbstractClassOfInt();
        var abstractClassOfString = container.CreateAbstractClassOfString();
        Console.WriteLine($"{iInterfaceClassOfInt.GetType().Name} {iInterfaceClassOfInt.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 Int32
        Console.WriteLine($"{iInterfaceOfString.GetType().Name} {iInterfaceOfString.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 String
        Console.WriteLine($"{abstractClassOfInt.GetType().Name} {abstractClassOfInt.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 Int32
        Console.WriteLine($"{abstractClassOfString.GetType().Name} {abstractClassOfString.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 String
    }
}