using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.GenericsAbstraction)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Generics.Abstraction;

internal interface IInterface<T>
{
}

internal abstract class AbstractClass<T>
{
}

internal class ConcreteClass<T> : AbstractClass<T>, IInterface<T>
{
}

[ImplementationAggregation(typeof(ConcreteClass<>))]
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
        // Do something with implementation
    }
}