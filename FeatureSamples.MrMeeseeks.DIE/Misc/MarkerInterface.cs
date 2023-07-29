using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.MiscMarkerInterface)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Misc.MarkerInterface;

// Define a marker interface for each dependency injection setting
public interface IContainerInstance { }
public interface ITransientScopeInstance { }
public interface IScopeInstance { }
public interface ITransientScopeRoot { }
public interface IScopeRoot { }
public interface ITransient { }
public interface ISyncTransient { }
public interface IAsyncTransient { }
// ReSharper disable once UnusedTypeParameter
public interface IDecorator<T> { }
// ReSharper disable once UnusedTypeParameter
public interface IComposite<T> { }
public interface IInitializer
{
    void Initialize();
}
public interface ITaskInitializer
{
    Task InitializeAsync();
}
public interface IValueTaskInitializer
{
    ValueTask InitializeAsync();
}

// A helpful interface (not marker interface)
internal interface IInterface {}

// Some implementations, composites and decorators which get marked with the marker interfaces.
internal class ImplementationA : IContainerInstance, IInitializer, ITransient, IDisposable, IAsyncDisposable
{
    public void Initialize() { }
    public void Dispose() { }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}

internal class ImplementationB : ITransientScopeInstance, IValueTaskInitializer, ISyncTransient, IDisposable
{
    public ValueTask InitializeAsync() => ValueTask.CompletedTask;
    public void Dispose() { }
}

internal class ImplementationC : IScopeInstance, ITaskInitializer, IAsyncTransient, IAsyncDisposable
{
    public Task InitializeAsync() => Task.CompletedTask;
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}

internal class Decorator : IInterface, IDecorator<IInterface>
{
    internal Decorator(IInterface decorated) {}
}

internal class Composite : IInterface, IComposite<IInterface>
{
    internal Composite(IEnumerable<IInterface> composited) {}
}

internal class TransientScopeRoot : ITransientScopeRoot
{
    internal TransientScopeRoot(IInterface dependency) {}
}

internal class ScopeRoot : IScopeRoot
{
    internal ScopeRoot(IInterface dependency) {}
}

internal class Root
{
    internal Root(
        IInterface dependency,
        TransientScopeRoot transientScopeRoot,
        ScopeRoot scopeRoot) {}
}

[ImplementationAggregation(typeof(ImplementationA), typeof(ImplementationB), typeof(ImplementationC), typeof(Decorator), typeof(Composite), typeof(TransientScopeRoot), typeof(ScopeRoot), typeof(Root))]

// Register marker interfaces instead of implementations for the diverse dependency injection settings.
[ContainerInstanceAbstractionAggregation(typeof(IContainerInstance))]
[TransientScopeInstanceAbstractionAggregation(typeof(ITransientScopeInstance))]
[ScopeInstanceAbstractionAggregation(typeof(IScopeInstance))]
[TransientScopeRootAbstractionAggregation(typeof(ITransientScopeRoot))]
[ScopeRootAbstractionAggregation(typeof(IScopeRoot))]
[TransientAbstractionAggregation(typeof(ITransient))]
[SyncTransientAbstractionAggregation(typeof(ISyncTransient))]
[AsyncTransientAbstractionAggregation(typeof(IAsyncTransient))]
[DecoratorAbstractionAggregation(typeof(IDecorator<>))]
[CompositeAbstractionAggregation(typeof(IComposite<>))]
[Initializer(typeof(IInitializer), nameof(IInitializer.Initialize))]
[Initializer(typeof(ITaskInitializer), nameof(ITaskInitializer.InitializeAsync))]
[Initializer(typeof(IValueTaskInitializer), nameof(IValueTaskInitializer.InitializeAsync))]

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
        // Checking would be overkill here.
    }
}