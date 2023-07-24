using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.IterablesIEnumerable)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Iterables.IEnumerableOfT;

// An interface and its implementations
internal interface IInterface
{
}

internal class ConcreteClass : IInterface
{
}

internal record Record : IInterface;

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Register the implementations mapping them to the interface
        builder.Services.AddTransient<IInterface, ConcreteClass>();
        builder.Services.AddTransient<IInterface, Record>();
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var iterable = host.Services.GetRequiredService<IEnumerable<IInterface>>();
        foreach (var item in iterable)
        {
            Console.WriteLine(item.GetType().Name);
        }
    }
}