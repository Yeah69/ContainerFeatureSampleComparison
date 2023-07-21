using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsStructRecord)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.StructRecord;

// Simple struct-record that we want to create objects from using a container
internal record struct StructRecord
{
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Register StructRecord and create a new value each time
        builder.Services.Add(new ServiceDescriptor(typeof(StructRecord), _ => new StructRecord(), ServiceLifetime.Transient));
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var value = host.Services.GetRequiredService<StructRecord>();
        Console.WriteLine(value.GetType().Name); // StructRecord
    }
}