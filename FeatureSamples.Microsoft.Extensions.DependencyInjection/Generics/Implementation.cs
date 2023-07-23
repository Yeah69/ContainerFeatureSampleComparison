using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.GenericsImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Generics.Implementation;

// Different types of generic implementations
internal class ConcreteClass<T>
{
}

internal record Record<T>
{
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Registering the generic types with the container
        builder.Services.Add(new ServiceDescriptor(typeof(ConcreteClass<>), typeof(ConcreteClass<>), ServiceLifetime.Transient));
        builder.Services.Add(new ServiceDescriptor(typeof(Record<>), typeof(Record<>), ServiceLifetime.Transient));
        
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var concreteClassOfInt = host.Services.GetRequiredService<ConcreteClass<int>>();
        Console.WriteLine($"{concreteClassOfInt.GetType().Name} {concreteClassOfInt.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 Int32
        var concreteClassOfString = host.Services.GetRequiredService<ConcreteClass<string>>();
        Console.WriteLine($"{concreteClassOfString.GetType().Name} {concreteClassOfString.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 String
        var recordOfInt = host.Services.GetRequiredService<Record<int>>();
        Console.WriteLine($"{recordOfInt.GetType().Name} {recordOfInt.GetType().GenericTypeArguments.First().Name}"); // Record`1 Int32
        var recordOfString = host.Services.GetRequiredService<Record<string>>();
        Console.WriteLine($"{recordOfString.GetType().Name} {recordOfString.GetType().GenericTypeArguments.First().Name}"); // Record`1 String
    }
}