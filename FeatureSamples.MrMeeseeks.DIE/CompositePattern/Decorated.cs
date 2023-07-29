using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CompositePatternDecorated)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.CompositePattern.Decorated;

// This is the interface of which the implementations will be composited and decorated.
internal interface IInterface
{
    IEnumerable<IInterface> Composition { get; }
}

// Multiple implementations of the interface.
internal class ComposedImplementationA : IInterface
{
    public IEnumerable<IInterface> Composition => new[] { this };
}

internal class ComposedImplementationB : IInterface
{
    public IEnumerable<IInterface> Composition => new[] { this };
}

internal class ComposedImplementationC : IInterface
{
    public IEnumerable<IInterface> Composition => new[] { this };
}

// This interface marks a decorator. Its generic parameter should be the decorated interface.
internal interface IDecorator<T> {}

// Multiple decorators of the interface.
internal class DecoratorA : IInterface, IDecorator<IInterface>
{
    internal DecoratorA(IInterface decorated) => Composition = new []{ decorated };
    public IEnumerable<IInterface> Composition { get; }
}

internal class DecoratorB : IInterface, IDecorator<IInterface>
{
    internal DecoratorB(IInterface decorated) => Composition = new []{ decorated };
    public IEnumerable<IInterface> Composition { get; }
}

internal class DecoratorC : IInterface, IDecorator<IInterface>
{
    internal DecoratorC(IInterface decorated) => Composition = new []{ decorated };
    public IEnumerable<IInterface> Composition { get; }
}

// This interface marks a composite. Its generic parameter should be the composited interface.
internal interface IComposite<T> {}

// This is the composite. It must implement the composited interface and the composite interface.
internal class Composite : IInterface, IComposite<IInterface>
{
    internal Composite(IEnumerable<IInterface> composition) => Composition = composition;
    public IEnumerable<IInterface> Composition { get; }
}

[ImplementationAggregation(typeof(Composite), typeof(ComposedImplementationA), typeof(ComposedImplementationB), typeof(ComposedImplementationC), typeof(DecoratorA), typeof(DecoratorB), typeof(DecoratorC))]
[DecoratorAbstractionAggregation(typeof(IDecorator<>))]
// We need to specify the composite interface.
[CompositeAbstractionAggregation(typeof(IComposite<>))]
// Specify the decorator sequences for the composited implementations.
[DecoratorSequenceChoice(typeof(IInterface), typeof(ComposedImplementationA), typeof(DecoratorA), typeof(DecoratorB), typeof(DecoratorC))]
[DecoratorSequenceChoice(typeof(IInterface), typeof(ComposedImplementationB), typeof(DecoratorC), typeof(DecoratorB), typeof(DecoratorA))]
[DecoratorSequenceChoice(typeof(IInterface), typeof(ComposedImplementationC))]
// The composite can be decorated, too.
[DecoratorSequenceChoice(typeof(IInterface), typeof(Composite), typeof(DecoratorB))]
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
        var root = container.Create();
        // Check the composition and the decorations
        Console.WriteLine(nameof(Composite));
        Console.WriteLine(root is DecoratorB); // True
        Console.WriteLine(root.Composition.First() is Composite); // True
        foreach (var instance in root.Composition.First().Composition)
        {
            if (instance is DecoratorC)
            {
                Console.WriteLine(nameof(ComposedImplementationA));
                Console.WriteLine(instance is DecoratorC); // True
                Console.WriteLine(instance.Composition.First() is DecoratorB); // True
                Console.WriteLine(instance.Composition.First().Composition.First() is DecoratorA); // True
                Console.WriteLine(instance.Composition.First().Composition.First().Composition.First() is ComposedImplementationA); // True
            }
            if (instance is DecoratorA)
            {
                Console.WriteLine(nameof(ComposedImplementationB));
                Console.WriteLine(instance is DecoratorA); // True
                Console.WriteLine(instance.Composition.First() is DecoratorB); // True
                Console.WriteLine(instance.Composition.First().Composition.First() is DecoratorC); // True
                Console.WriteLine(instance.Composition.First().Composition.First().Composition.First() is ComposedImplementationB); // True
            }
            if (instance is ComposedImplementationC)
            {
                Console.WriteLine(nameof(ComposedImplementationC));
                Console.WriteLine(instance is ComposedImplementationC); // True
            }
        }
    }
}