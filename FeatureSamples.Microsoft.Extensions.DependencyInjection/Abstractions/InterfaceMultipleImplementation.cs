using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.AbstractionsInterfaceMultipleImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Abstractions.InterfaceMultipleImplementation;

// Simple interface that will work as an abstraction for an implementation
internal interface IInterface
{
}

// Simple class that will be used as an implementation for the interface
internal class ConcreteClass : IInterface
{
}

// Another simple class that implements the interface
internal class AnotherConcreteClass : IInterface
{
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Explicitly choose ConcreteClass as the implementation for IInterface
        builder.Services.AddTransient<IInterface, ConcreteClass>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var concreteClass = host.Services.GetRequiredService<IInterface>();
        Console.WriteLine(concreteClass.GetType().Name); // ConcreteClass
    }
}