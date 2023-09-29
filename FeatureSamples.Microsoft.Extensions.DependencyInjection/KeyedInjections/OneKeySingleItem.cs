using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.KeyedInjectionsOneKeySingleItem)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.KeyedInjections.OneKeySingleItem;

// Simple enum that will be used as a key for the injections
internal enum Key
{
    A,
    B
}

// Simple interface that will work as an abstraction for the implementations
internal interface IInterface {}

// Implementation that'll get the key A assigned during registration
internal class ConcreteClassA : IInterface {}

// Implementation that'll get the key B assigned during registration
internal class ConcreteClassB : IInterface {}

internal class Parent
{
    internal IInterface Dependency { get; }
    
    // Inject a single implementation for the key A
    public Parent([FromKeyedServices(Key.A)] IInterface dependency) => Dependency = dependency;
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<Parent>();
        
        // Register the implementations with the appropriate keys
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(Key.A);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(Key.B);
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var instance = host.Services.GetRequiredService<Parent>();
        Console.WriteLine(instance.Dependency.GetType().Name); // ConcreteClassA
    }
}