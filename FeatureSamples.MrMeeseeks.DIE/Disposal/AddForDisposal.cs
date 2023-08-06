using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.DisposalAddForDisposal)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Disposal.AddForDisposal;

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

internal class TransientScopeRoot
{
    internal TransientScopeRoot(
        ConcreteClassSyncTransient concreteClassSyncTransient,
        ConcreteClassAsyncTransient concreteClassAsyncTransient,
        System.IAsyncDisposable transientScopeDisposalHandle)
    {
        ConcreteClassSyncTransient = concreteClassSyncTransient;
        ConcreteClassAsyncTransient = concreteClassAsyncTransient;
        TransientScopeDisposalHandle = transientScopeDisposalHandle;
    }
    internal ConcreteClassSyncTransient ConcreteClassSyncTransient { get; }
    internal ConcreteClassAsyncTransient ConcreteClassAsyncTransient { get; }
    internal System.IAsyncDisposable TransientScopeDisposalHandle { get; }
}

internal class Root
{
    internal Root(
        ConcreteClassSync concreteClassSync,
        ConcreteClassAsync concreteClassAsync,
        TransientScopeRoot transientScopeRoot)
    {
        ConcreteClassSync = concreteClassSync;
        ConcreteClassAsync = concreteClassAsync;
        TransientScopeRoot = transientScopeRoot;
    }
    internal ConcreteClassSync ConcreteClassSync { get; }
    internal ConcreteClassAsync ConcreteClassAsync { get; }
    internal TransientScopeRoot TransientScopeRoot { get; }
}

[ImplementationAggregation(typeof(TransientScopeRoot), typeof(Root))]
[TransientScopeRootImplementationAggregation(typeof(TransientScopeRoot))]
[CreateFunction(typeof(Root), "Create")]
internal partial class Container
{
    private Container() {}
    
    // Prepare partial function signature for a generated function that'll append sync disposables for the container to manage.
    private partial void DIE_AddForDisposal(System.IDisposable disposable);
    // Prepare partial function signature for a generated function that'll append async disposables for the container to manage.
    private partial void DIE_AddForDisposalAsync(System.IAsyncDisposable asyncDisposable);
    
    private ConcreteClassSync DIE_Factory_ConcreteClassSync()
    {
        // This instance isn't created by the container itself but we would like to get it disposed with the container.
        var instance = new ConcreteClassSync();
        // So we add it to the container's disposal list.
        DIE_AddForDisposal(instance);
        return instance;
    }

    private ConcreteClassAsync DIE_Factory_ConcreteClassAsync()
    {
        // This instance isn't created by the container itself but we would like to get it disposed with the container.
        var instance = new ConcreteClassAsync();
        // So we add it to the container's disposal list.
        DIE_AddForDisposalAsync(instance);
        return instance;
    }

    private partial class DIE_DefaultTransientScope
    {
        // Prepare partial function signature for a generated function that'll append sync disposables for the transient scope to manage.
        private partial void DIE_AddForDisposal(System.IDisposable disposable);
        // Prepare partial function signature for a generated function that'll append async disposables for the transient scope to manage.
        private partial void DIE_AddForDisposalAsync(System.IAsyncDisposable asyncDisposable);
    
        private ConcreteClassSyncTransient DIE_Factory_ConcreteClassSyncTransient()
        {
            // This instance isn't created by the transient scope itself but we would like to get it disposed with the transient scope.
            var instance = new ConcreteClassSyncTransient();
            // So we add it to the transient scope's disposal list.
            DIE_AddForDisposal(instance);
            return instance;
        }

        private ConcreteClassAsyncTransient DIE_Factory_ConcreteClassAsyncTransient()
        {
            // This instance isn't created by the transient scope itself but we would like to get it disposed with the transient scope.
            var instance = new ConcreteClassAsyncTransient();
            // So we add it to the transient scope's disposal list.
            DIE_AddForDisposalAsync(instance);
            return instance;
        }
    }
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        var container = Container.DIE_CreateContainer();
        var root = container.Create();
        
        Console.WriteLine($"Disposed: {root.ConcreteClassSync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {root.TransientScopeRoot.ConcreteClassSyncTransient.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {root.ConcreteClassAsync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {root.TransientScopeRoot.ConcreteClassAsyncTransient.Disposed}"); // Disposed: False

        // Eagerly dispose the transient scope root
        await root.TransientScopeRoot.TransientScopeDisposalHandle.DisposeAsync().ConfigureAwait(false);
        
        // The instances from the transient scope root are disposed
        Console.WriteLine($"Disposed: {root.ConcreteClassSync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {root.TransientScopeRoot.ConcreteClassSyncTransient.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {root.ConcreteClassAsync.Disposed}"); // Disposed: False
        Console.WriteLine($"Disposed: {root.TransientScopeRoot.ConcreteClassAsyncTransient.Disposed}"); // Disposed: True

        await container.DisposeAsync();
        
        Console.WriteLine($"Disposed: {root.ConcreteClassSync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {root.TransientScopeRoot.ConcreteClassSyncTransient.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {root.ConcreteClassAsync.Disposed}"); // Disposed: True
        Console.WriteLine($"Disposed: {root.TransientScopeRoot.ConcreteClassAsyncTransient.Disposed}"); // Disposed: True
    }
}