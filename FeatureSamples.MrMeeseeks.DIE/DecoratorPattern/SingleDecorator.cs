using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DecoratorPatternSingleDecorator)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.DecoratorPattern.SingleDecorator;

// This is the interface which will be decorated.
internal interface IInterface
{
    IInterface Decorated { get; }
}

// This is the implementation that will be decorated.
internal class DecoratedImplementation : IInterface
{
    public IInterface Decorated => this;
}

// This interface marks a decorator. Its generic parameter should be the decorated interface.
internal interface IDecorator<T> {}

// This is the decorator. It must implement the decorated interface and the decorator interface.
internal class Decorator : IInterface, IDecorator<IInterface>
{
    // Also it can have a dependency of the decorated interface. The decorated implementation instance or another decorator will be injected here.
    internal Decorator(IInterface decorated) => Decorated = decorated;
    public IInterface Decorated { get; }
}

[ImplementationAggregation(typeof(DecoratedImplementation), typeof(Decorator))]
// We need to specify the decorator interface.
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
        // The topmost instance is the decorator.
        Console.WriteLine(instance is Decorator); // True
        // The decorated instance is nested.
        Console.WriteLine(instance.Decorated is DecoratedImplementation); // True
    }
}