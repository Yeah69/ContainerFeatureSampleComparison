using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ScopedInstancesContainer)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.ScopedInstances.Container;

// Simple class that will be created once per container
internal class ConcreteClass { }

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Register the class as singleton
        builder.Services.AddSingleton<ConcreteClass>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        // All resolved instances are equal
        var concreteClassA = host.Services.GetRequiredService<ConcreteClass>();
        var concreteClassB = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine(concreteClassA == concreteClassB); // True

        using var scope = host.Services.CreateScope();
        var concreteClassC = scope.ServiceProvider.GetRequiredService<ConcreteClass>();
        var concreteClassD = scope.ServiceProvider.GetRequiredService<ConcreteClass>();
            
        Console.WriteLine(concreteClassC == concreteClassD); // True
        Console.WriteLine(concreteClassC == concreteClassA); // True

        using var scopeNested = host.Services.CreateScope();
        var concreteClassE = scopeNested.ServiceProvider.GetRequiredService<ConcreteClass>();
        var concreteClassF = scopeNested.ServiceProvider.GetRequiredService<ConcreteClass>();
                
        Console.WriteLine(concreteClassE == concreteClassF); // True
        Console.WriteLine(concreteClassE == concreteClassC); // True
        Console.WriteLine(concreteClassE == concreteClassA); // True
    }
}
