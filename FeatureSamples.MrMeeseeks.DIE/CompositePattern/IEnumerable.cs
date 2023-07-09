using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CompositePatternIEnumerable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.CompositePattern.IEnumerable;

internal interface IInterface
{
    IEnumerable<IInterface> Composition { get; }
}

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

internal interface IComposite<T> {}

internal class Composite : IInterface, IComposite<IInterface>
{
    internal Composite(IEnumerable<IInterface> composition) => Composition = composition;
    public IEnumerable<IInterface> Composition { get; }
}

[ImplementationAggregation(typeof(ComposedImplementationA), typeof(ComposedImplementationB), typeof(ComposedImplementationC), typeof(Composite))]
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
        Console.WriteLine(instance.GetType().Name);
        foreach (var @interface in instance.Composition)
        {
            Console.WriteLine(@interface.GetType().Name);
        }
    }
}