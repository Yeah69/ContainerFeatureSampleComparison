using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DecoratorPatternMultipleDecorators)]
[assembly:FeatureSample(Feature.DecoratorPatternExplicitOrder)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.DecoratorPattern.MultipleDecorators;

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

// This time we define multiple decorators.
internal class DecoratorA : IInterface, IDecorator<IInterface>
{
    internal DecoratorA(IInterface decorated) => Decorated = decorated;
    public IInterface Decorated { get; }
}

internal class DecoratorB : IInterface, IDecorator<IInterface>
{
    internal DecoratorB(IInterface decorated) => Decorated = decorated;
    public IInterface Decorated { get; }
}

internal class DecoratorC : IInterface, IDecorator<IInterface>
{
    internal DecoratorC(IInterface decorated) => Decorated = decorated;
    public IInterface Decorated { get; }
}

[ImplementationAggregation(typeof(DecoratedImplementation), typeof(DecoratorA), typeof(DecoratorB), typeof(DecoratorC))]
// We need to specify the decorator interface.
[DecoratorAbstractionAggregation(typeof(IDecorator<>))]
// We define an explicit order for the decorators with the following attribute.
// First parameter should be the decorated interface. The second parameter should be the type for which we want to set a decoration order.
// Then a list of decorators sorted from innermost to outermost decorator.
[DecoratorSequenceChoice(typeof(IInterface), typeof(DecoratedImplementation), typeof(DecoratorA), typeof(DecoratorB), typeof(DecoratorC))]
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
        // Checking the decoration order
        Console.WriteLine(instance is DecoratorC); // True
        Console.WriteLine(instance.Decorated is DecoratorB); // True
        Console.WriteLine(instance.Decorated.Decorated is DecoratorA); // True
        Console.WriteLine(instance.Decorated.Decorated.Decorated is DecoratedImplementation); // True
    }
}