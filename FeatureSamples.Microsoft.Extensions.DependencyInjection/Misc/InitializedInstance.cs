using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.MiscInitializedInstance)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Misc.InitializedInstance;

// Simple class that we want to create objects from using a container
internal class ConcreteClass { }

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();
        
        // Create in instance manually and register it as a singleton to make it an initialized instance on container level
        var initializedInstance = new ConcreteClass();
        builder.Services.AddSingleton(initializedInstance);
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var initializedInstanceA = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine(initializedInstanceA.GetType().Name); // ConcreteClass
        
        var initializedInstanceB = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine(initializedInstanceA == initializedInstanceB); // True
    }
}