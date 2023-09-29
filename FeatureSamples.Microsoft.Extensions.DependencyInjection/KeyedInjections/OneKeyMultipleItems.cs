using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.KeyedInjectionsOneKeyMultipleItems)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.KeyedInjections.OneKeyMultipleItems;

// Simple enum that will be used as a key for the injections
internal enum Key
{
    A,
    B
}

// Simple interface that will work as an abstraction for the implementations
internal interface IInterface {}

// Implementations that'll get the key A assigned during registration
internal class ConcreteClassA0 : IInterface {}
internal class ConcreteClassA1 : IInterface {}

// Implementations that'll get the key B assigned during registration
internal class ConcreteClassB0 : IInterface {}
internal class ConcreteClassB1 : IInterface {}

internal class Parent
{
    internal IInterface[] Dependencies { get; }
    
    // Inject a multiple implementations for the key A
    public Parent([FromKeyedServices(Key.A)] IEnumerable<IInterface> dependencies) => 
        Dependencies = dependencies.ToArray();
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<Parent>();
        
        // Register the implementations with the appropriate keys
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA0>(Key.A);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA1>(Key.A);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB0>(Key.B);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB1>(Key.B);
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var instance = host.Services.GetRequiredService<Parent>();
        
        Console.WriteLine(instance.Dependencies.Length); // 2
        foreach (var dependency in instance.Dependencies)
            Console.WriteLine(dependency is ConcreteClassA0 or ConcreteClassA1); // True
    }
}