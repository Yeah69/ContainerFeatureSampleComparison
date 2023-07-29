using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Disposal.IAsyncDisposable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.DisposalScope)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Disposal.Scope;

// Two simple classes that are either synchronously or asynchronously disposable
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

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<ConcreteClass>();
        builder.Services.AddTransient<ConcreteClassAsync>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        using var host = IAsyncDisposable.Builder.CreateBuilder().Build();

        var syncScope = host.Services.CreateScope();
        var syncConcreteClass = syncScope.ServiceProvider.GetRequiredService<ConcreteClassSync>();
        
        Console.WriteLine($"Disposed: {syncConcreteClass.Disposed}"); // Disposed: False
        // As soon as the scope is disposed, all its managed disposable dependencies are disposed as well.
        syncScope.Dispose();
        Console.WriteLine($"Disposed: {syncConcreteClass.Disposed}"); // Disposed: True

        var asyncScope = host.Services.CreateAsyncScope();
        var asyncConcreteClass = syncScope.ServiceProvider.GetRequiredService<ConcreteClassAsync>();
        
        Console.WriteLine($"Disposed: {asyncConcreteClass.Disposed}"); // Disposed: False
        // As soon as the scope is disposed, all its managed disposable dependencies are disposed as well.
        await asyncScope.DisposeAsync();
        Console.WriteLine($"Disposed: {asyncConcreteClass.Disposed}"); // Disposed: True
    }
}