using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalScope)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.Scope;

// Some simple disposable classes for which the disposal will be either managed by the container/scope or not.
internal class ConcreteClassSync : System.IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

internal class ConcreteClassAsync : System.IAsyncDisposable
{
    internal bool Disposed { get; private set; }
    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}

internal class ConcreteClassSyncTransient : System.IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

internal class ConcreteClassAsyncTransient : System.IAsyncDisposable
{
    internal bool Disposed { get; private set; }
    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}

// Inject them all into the transient scope root.
internal class TransientScopeRoot
{
    internal TransientScopeRoot(
        ConcreteClassSync concreteClassSync,
        ConcreteClassSyncTransient concreteClassSyncTransient,
        ConcreteClassAsync concreteClassAsync,
        ConcreteClassAsyncTransient concreteClassAsyncTransient,
        System.IAsyncDisposable transientScopeDisposalHandle)
    {
        ConcreteClassSync = concreteClassSync;
        ConcreteClassSyncTransient = concreteClassSyncTransient;
        ConcreteClassAsync = concreteClassAsync;
        ConcreteClassAsyncTransient = concreteClassAsyncTransient;
        TransientScopeDisposalHandle = transientScopeDisposalHandle;
    }
    
    internal ConcreteClassSync ConcreteClassSync { get; }
    internal ConcreteClassSyncTransient ConcreteClassSyncTransient { get; }
    internal ConcreteClassAsync ConcreteClassAsync { get; }
    internal ConcreteClassAsyncTransient ConcreteClassAsyncTransient { get; }
    internal System.IAsyncDisposable TransientScopeDisposalHandle { get; }
}

[ImplementationAggregation(typeof(TransientScopeRoot), typeof(ConcreteClassSync), typeof(ConcreteClassSyncTransient), typeof(ConcreteClassAsync), typeof(ConcreteClassAsyncTransient))]
// Make half of the transient/unmanaged
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
[TransientImplementationAggregation(typeof(ConcreteClassSyncTransient), typeof(ConcreteClassAsyncTransient))]
[CreateFunction(typeof(TransientScopeRoot), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        await using var container = Container.DIE_CreateContainer();
        var transientScopeRoot = container.Create();
        
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassSync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassSyncTransient.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassAsync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassAsyncTransient.Disposed}"); // Disposed: False

        // Eagerly dispose the transient scope root
        await transientScopeRoot.TransientScopeDisposalHandle.DisposeAsync().ConfigureAwait(false);
        
        // The managed instances are disposed
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassSync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassSyncTransient.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassAsync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassAsyncTransient.Disposed}"); // Disposed: False
        
        // Just because it is good manners to dispose everything ;)
        transientScopeRoot.ConcreteClassSyncTransient.Dispose();
        await transientScopeRoot.ConcreteClassAsyncTransient.DisposeAsync();
        
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassSync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassSyncTransient.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassAsync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {transientScopeRoot.ConcreteClassAsyncTransient.Disposed}"); // Disposed: True
    }
}