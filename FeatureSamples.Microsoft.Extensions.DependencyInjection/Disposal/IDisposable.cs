using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.DisposalIDisposable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Disposal.IDisposable;

// This simple class is disposable
internal class ConcreteClass : System.IDisposable
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
        var host = Builder.CreateBuilder().Build();

        var concreteClass = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: False
        // As soon as the container is disposed, all its managed disposable dependencies are disposed as well.
        host.Dispose();
        Console.WriteLine($"Disposed: {concreteClass.Disposed}"); // Disposed: True
    }
}