using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.InjectionsTypeInitializerParameter)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Injections.TypeInitializerParameter;

// Simple class that we want to inject into another class
internal class ConcreteClass {}

internal class Parent
{
    internal ConcreteClass? Dependency { get; private set; }
    internal void Initialize(ConcreteClass dependency) => Dependency = dependency;
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<ConcreteClass>();
        builder.Services.AddTransient<Parent>(sp =>
        {
            // Don't use sp.GetService<Parent>() or sp.GetRequiredService<Parent>() here because that would create an infinite loop
            var parent = ActivatorUtilities.CreateInstance<Parent>(sp);
            var concreteClass = sp.GetRequiredService<ConcreteClass>();
            parent.Initialize(concreteClass);
            return parent;
        });
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var parent = host.Services.GetRequiredService<Parent>();
        Console.WriteLine(parent.Dependency!.GetType().Name); // ConcreteClass
    }
}