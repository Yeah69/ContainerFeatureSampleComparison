using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsRecord)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.Record;

internal record Record;

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<Record>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var record = host.Services.GetRequiredService<Record>();
        // Do something with the instance
    }
}