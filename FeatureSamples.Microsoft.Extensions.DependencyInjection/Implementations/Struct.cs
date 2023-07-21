using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsStruct)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.Struct;

// Simple struct that we want to create values from using a container
internal struct Struct
{
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();
        
        // Register Struct and create a new value each time
        builder.Services.Add(new ServiceDescriptor(typeof(Struct), _ => new Struct(), ServiceLifetime.Transient));
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var value = host.Services.GetRequiredService<Struct>();
        Console.WriteLine(value.GetType().Name); // Struct
    }
}