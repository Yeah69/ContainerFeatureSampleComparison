using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.AbstractionsNullableAbstractClassNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Abstractions.NullableAbstractClassNullCase;

// Simple abstract class that doesn't have any implementation
internal abstract class AbstractClass
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    public Parent(AbstractClass? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Explicitly choose null for AbstractClass
        builder.Services.AddTransient<AbstractClass>(_ => null!);
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
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: True
    }
}