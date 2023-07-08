using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DecoratorPatternSingleDecorator)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.DecoratorPattern.SingleDecorator;

internal interface IInterface
{
    IInterface Decorated { get; }
}

internal class DecoratedImplementation : IInterface
{
    public IInterface Decorated => this;
}

internal interface IDecorator<T> {}

internal class Decorator : IInterface, IDecorator<IInterface>
{
    internal Decorator(IInterface decorated) => Decorated = decorated;
    public IInterface Decorated { get; }
}

[ImplementationAggregation(typeof(DecoratedImplementation), typeof(Decorator))]
[DecoratorAbstractionAggregation(typeof(IDecorator<>))]
[CreateFunction(typeof(IInterface), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var instance = container.Create();
        Console.WriteLine(instance is Decorator); // True
        Console.WriteLine(instance.Decorated is DecoratedImplementation); // True
        // Do something with implementation
    }
}