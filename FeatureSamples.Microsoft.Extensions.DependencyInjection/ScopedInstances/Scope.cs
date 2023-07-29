using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ScopedInstancesScope)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.ScopedInstances.Scope;

// This simple class will be shared per scope
internal class ConcreteClass { }

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Register the class as scoped
        builder.Services.AddScoped<ConcreteClass>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        // The topmost resolved instances are equal
        var concreteClassA = host.Services.GetRequiredService<ConcreteClass>();
        var concreteClassB = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine(concreteClassA == concreteClassB); // True

        using var scope = host.Services.CreateScope();
        var concreteClassC = scope.ServiceProvider.GetRequiredService<ConcreteClass>();
        var concreteClassD = scope.ServiceProvider.GetRequiredService<ConcreteClass>();
            
        // The scope instances are equal, but not to the topmost instance
        Console.WriteLine(concreteClassC == concreteClassD); // True
        Console.WriteLine(concreteClassC == concreteClassA); // False

        using var scopeNested = host.Services.CreateScope();
        var concreteClassE = scopeNested.ServiceProvider.GetRequiredService<ConcreteClass>();
        var concreteClassF = scopeNested.ServiceProvider.GetRequiredService<ConcreteClass>();
        
        // The nested scope instances are equal, but not to the topmost or the other scope instance
        Console.WriteLine(concreteClassE == concreteClassF); // True
        Console.WriteLine(concreteClassE == concreteClassC); // False
        Console.WriteLine(concreteClassE == concreteClassA); // False
    }
}