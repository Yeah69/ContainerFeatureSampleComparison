using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsExplicitConstructorChoice)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.ExplicitConstructorChoice;

// This class has three constructors
// That means that it is ambiguous which constructor to choose
internal class ConcreteClass
{
    public ConcreteClass() => Char = 'a';
    // This attribute tells the container to use this constructor
    [ActivatorUtilitiesConstructor]
    public ConcreteClass(double _) => Char = 'b';
    public ConcreteClass(DateTime _, FileInfo __) => Char = 'c';
    internal char Char { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Register the dependency for the chosen constructor
        builder.Services.Add(new ServiceDescriptor(typeof(double), 1.0));
        
        // Register the dependencies for the other constructors. So in principle, we could have chosen any of the constructors
        builder.Services.Add(new ServiceDescriptor(typeof(DateTime), DateTime.Now));
        builder.Services.Add(new ServiceDescriptor(typeof(FileInfo), new FileInfo("file.txt")));
        
        // Register the class itself and use the ActivatorUtilities to create an instance
        builder.Services.AddTransient<ConcreteClass>(sp => ActivatorUtilities.CreateInstance<ConcreteClass>(sp));
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var concreteClass = host.Services.GetRequiredService<ConcreteClass>();
        Console.WriteLine($"Char: {concreteClass.Char}"); // Char: b
    }
}