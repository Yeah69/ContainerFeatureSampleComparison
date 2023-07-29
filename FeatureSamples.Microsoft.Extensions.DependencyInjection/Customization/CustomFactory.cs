using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.CustomizationCustomFactory)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.Customization.CustomFactory;

// This class won't be created by the container but by a custom factory.
internal class Person
{
    internal Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    internal string Name { get; }
    public int Age { get; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        // Use the factory parameter in order to customize the creation of the instance.
        builder.Services.AddTransient<Person>(_ => new Person("Jane Doe", 42));
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        using var host = Builder.CreateBuilder().Build();

        var person = host.Services.GetRequiredService<Person>();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Name: Jane Doe, Age: 42
    }
}