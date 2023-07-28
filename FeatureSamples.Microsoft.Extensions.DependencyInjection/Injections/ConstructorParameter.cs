using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.InjectionsConstructorParameter)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Injections.ConstructorParameter;

// Simple class that we want to inject into another class
internal class ConcreteClass
{
}

internal class Parent
{
    // Inject it here as a constructor parameter
    internal Parent(ConcreteClass child) => Dependency = child;
    internal ConcreteClass Dependency { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<ConcreteClass>();
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
        Console.WriteLine(parent.Dependency.GetType().Name); // ConcreteClass
    }
}