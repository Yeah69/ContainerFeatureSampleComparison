using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DecoratorPatternMultipleDecorators)]
[assembly:FeatureSample(Feature.DecoratorPatternExplicitOrder)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.DecoratorPattern.MultipleDecorators;

internal interface IInterface
{
    IInterface Decorated { get; }
}

internal class DecoratedImplementation : IInterface
{
    public IInterface Decorated => this;
}

internal interface IDecorator<T> {}

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
[DecoratorAbstractionAggregation(typeof(IDecorator<>))]
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
        Console.WriteLine(instance is DecoratorC); // True
        Console.WriteLine(instance.Decorated is DecoratorB); // True
        Console.WriteLine(instance.Decorated.Decorated is DecoratorA); // True
        Console.WriteLine(instance.Decorated.Decorated.Decorated is DecoratedImplementation); // True
        // Do something with implementation
    }
}