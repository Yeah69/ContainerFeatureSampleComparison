using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.AbstractionsAbstractClassMultipleImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Abstractions.AbstractClassMultipleImplementation;

// Simple abstract class that will work as an abstraction for a concrete class
internal abstract class AbstractClass
{
}

// Simple class that will be used as an implementation for the abstract class
internal class ConcreteClass : AbstractClass
{
}

// Another simple class that implements the abstract class
internal class AnotherConcreteClass : AbstractClass
{
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Explicitly choose ConcreteClass as the implementation for AbstractClass
        builder.Services.AddTransient<AbstractClass, ConcreteClass>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var concreteClass = host.Services.GetRequiredService<AbstractClass>();
        Console.WriteLine(concreteClass.GetType().Name); // ConcreteClass
    }
}