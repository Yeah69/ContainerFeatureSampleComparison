using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.ImplementationsExplicitConstructorChoice)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Implementations.ExplicitConstructorChoice;

internal class ConcreteClass
{
    public ConcreteClass() => Char = 'a';
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

        builder.Services.Add(new ServiceDescriptor(typeof(double), 1.0));
        builder.Services.Add(new ServiceDescriptor(typeof(DateTime), DateTime.Now));
        builder.Services.Add(new ServiceDescriptor(typeof(FileInfo), new FileInfo("file.txt")));
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
        // Do something with the instance
    }
}