using System.Reflection;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.MiscRegisterAllImplementations)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Misc.RegisterAllImplementations;

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        var assembly = typeof(Usage).Assembly;
        var types = assembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Prepend(assembly)
            .SelectMany(a => a.GetTypes())
            .ToArray();
        var implementations = types
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .ToArray();
        
        foreach (var implementation in implementations)
        {
            builder.Services.AddTransient(implementation);
        }

        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();
        
        var featureEnumInfoAttribute = host.Services.GetRequiredService<FeatureEnumInfoAttribute>();
        Console.WriteLine(featureEnumInfoAttribute.GetType().Name); // FeatureEnumInfoAttribute
    }
}