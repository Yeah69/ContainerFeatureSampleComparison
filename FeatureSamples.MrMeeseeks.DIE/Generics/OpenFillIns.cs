using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.GenericsOpenFillIns)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Generics.OpenFillIns;

internal interface IInterface<T>
{
}

internal class ConcreteClass<T, TA> : IInterface<T>
{
}

[ImplementationAggregation(typeof(ConcreteClass<,>))]
[GenericParameterChoice(typeof(ConcreteClass<,>), "TA", typeof(string))]
[GenericParameterSubstitutesChoice(typeof(ConcreteClass<,>), "TA", typeof(bool), typeof(double))]
[CreateFunction(typeof(IInterface<int>), "CreateSingular")]
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
        var singular = container.CreateSingular(); // ConcreteClass<int, string>
        // The multiple option will fill the open generic parameter with string, bool and double
        // That means the singular option is implicitly included in the multiple option
        var multiple = container.CreateMultiple(); // ConcreteClass<int, string>[], ConcreteClass<int, bool>[], ConcreteClass<int, double>[]
        // Do something with implementation
    }
}