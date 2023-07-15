using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsNullableStructNotNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.NullableStructNotNullCase;

internal struct Struct
{
}

internal class Parent
{
    public Parent(Struct? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.Add(new ServiceDescriptor(typeof(Struct?), _ => new Struct(), ServiceLifetime.Transient));
        builder.Services.AddTransient<Parent>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var parent = host.Services.GetRequiredService<Parent>();
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: False
        // Do something with the instance
    }
}