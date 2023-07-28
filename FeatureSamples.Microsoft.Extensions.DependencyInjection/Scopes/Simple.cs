using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ScopesSimple)]
[assembly:FeatureSample(Feature.ScopesTransient)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Scopes.Simple;

// Simple class that will be instantiated inside a scope
internal class ConcreteClass : IDisposable
{
    internal bool Disposed { get; private set; }
    public void Dispose() => Disposed = true;
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<ConcreteClass>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        // Create a new scope
        var scope = host.Services.CreateScope();

        var concreteClass = scope.ServiceProvider.GetRequiredService<ConcreteClass>();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        // Disposing the scope will dispose all its managed disposable dependencies.
        scope.Dispose();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}
