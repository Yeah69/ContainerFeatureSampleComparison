using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsNullableConcreteClassNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.NullableConcreteClassNullCase;

internal class ConcreteClass
{
}

internal class Parent
{
    public Parent(ConcreteClass? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<ConcreteClass>(_ => null!);
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
        // Do something with the instance
    }
}