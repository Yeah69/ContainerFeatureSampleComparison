using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CompositePatternIEnumerable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.CompositePattern.IEnumerable;

// This is the interface of which the implementations will be composited.
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

// This interface marks a composite. Its generic parameter should be the composited interface.
internal interface IComposite<T> {}

// This is the composite. It must implement the composited interface and the composite interface.
internal class Composite : IInterface, IComposite<IInterface>
{
    // Also it can have a iterable dependency of the composited interface. The composited implementation instances will be injected here.
    internal Composite(IEnumerable<IInterface> composition) => Composition = composition;
    public IEnumerable<IInterface> Composition { get; }
}

[ImplementationAggregation(typeof(ComposedImplementationA), typeof(ComposedImplementationB), typeof(ComposedImplementationC), typeof(Composite))]
// We need to specify the composite interface.
[CompositeAbstractionAggregation(typeof(IComposite<>))]
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
        // Check the types of the composite and its composition.
        Console.WriteLine(instance.GetType().Name);
        foreach (var @interface in instance.Composition)
        {
            Console.WriteLine(@interface.GetType().Name);
        }
    }
}