﻿using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DecoratorPatternDefaultOrder)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.DecoratorPattern.DefaultOrder;

internal interface IInterface
{
    IInterface Decorated { get; }
}

internal class DecoratedImplementationA : IInterface
{
    public IInterface Decorated => this;
}

internal class DecoratedImplementationB : IInterface
{
    public IInterface Decorated => this;
}

internal class DecoratedImplementationC : IInterface
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

[ImplementationAggregation(typeof(DecoratedImplementationA), typeof(DecoratedImplementationB), typeof(DecoratedImplementationC), typeof(DecoratorA), typeof(DecoratorB), typeof(DecoratorC))]
[DecoratorAbstractionAggregation(typeof(IDecorator<>))]
[DecoratorSequenceChoice(typeof(IInterface), typeof(DecoratedImplementationA), typeof(DecoratorA), typeof(DecoratorB), typeof(DecoratorC))]
[DecoratorSequenceChoice(typeof(IInterface), typeof(IInterface), typeof(DecoratorB))]
[CreateFunction(typeof(IEnumerable<IInterface>), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var instances = container.Create();
        foreach (var instance in instances)
        {
            if (instance is DecoratorC)
            {
                Console.WriteLine(nameof(DecoratedImplementationA));
                Console.WriteLine(instance is DecoratorC); // True
                Console.WriteLine(instance.Decorated is DecoratorB); // True
                Console.WriteLine(instance.Decorated.Decorated is DecoratorA); // True
                Console.WriteLine(instance.Decorated.Decorated.Decorated is DecoratedImplementationA); // True
            }
            if (instance is DecoratorB)
            {
                Console.WriteLine(instance.Decorated.GetType().Name);
                Console.WriteLine(instance is DecoratorB); // True
                Console.WriteLine(instance.Decorated is DecoratedImplementationB or DecoratedImplementationC); // True
            }
        }
        
        // Do something with implementation
    }
}