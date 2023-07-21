using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.AbstractionsNullableAbstractClassNotNullCase)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Abstractions.NullableAbstractClassNotNullCase;

// Simple abstract class that will work as an abstraction for a concrete class
internal abstract class AbstractClass
{
}

// Simple class that will be used as an implementation for the abstract class
internal class ConcreteClass : AbstractClass
{
}

// Utility class to get a nullable injection and check if it is null
internal class Parent
{
    public Parent(AbstractClass? child) => IsNull = child is null; 
    internal bool IsNull { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Explicitly choose ConcreteClass as the implementation for AbstractClass
        builder.Services.AddTransient<AbstractClass, ConcreteClass>();
        builder.Services.AddTransient<Parent>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var parent = host.Services.GetRequiredService<Parent>();
        Console.WriteLine($"Is null: {parent.IsNull}"); // Is null: False
    }
}