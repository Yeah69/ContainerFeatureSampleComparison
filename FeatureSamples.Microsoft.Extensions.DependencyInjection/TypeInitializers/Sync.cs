using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.TypeInitializersSync)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.TypeInitializers.Sync;

internal class ConcreteClass
{
    internal bool Initialized { get; private set; }
    // This method should be called after the instance is created and before it is further injected
    internal void Initialize() => Initialized = true;
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Use the factory parameter in order to call the Initialize-method after the instance is created
        builder.Services.AddTransient<ConcreteClass>(sp =>
        {
            // Don't use sp.GetService<ConcreteClass>() or sp.GetRequiredService<ConcreteClass>() here because that would create an infinite loop
            var concreteClass = ActivatorUtilities.CreateInstance<ConcreteClass>(sp);
            concreteClass.Initialize();
            return concreteClass;
        });
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var concreteClass = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine($"Initialized: {concreteClass.Initialized}"); // Initialized: True
    }
}