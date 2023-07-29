using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomFactory)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomFactory;

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

// This class will be created by the container but its dependencies will be created by custom factories.
internal class RandomData
{
    internal RandomData(decimal randomDecimal, char randomChar)
    {
        RandomDecimal = randomDecimal;
        RandomChar = randomChar;
    }

    public char RandomChar { get; }

    public decimal RandomDecimal { get; }
}

[ImplementationAggregation(typeof(RandomData))]
[CreateFunction(typeof(Person), "Create")]
[CreateFunction(typeof(RandomData), "CreateRandomData")]
internal partial class Container
{
    // Custom factories are members of the container class. Their name must start with "DIE_Factory".
    // They can be fields, …
    private int DIE_Factory_Age = 42;
    
    private Container() {}
    
    // … properties with a get-accessor …
    private string DIE_Factory_Name => "Jane Doe";
    
    // … and functions.
    // The function variant can have dependencies itself, that it can use to create the instance.
    private Person DIE_Factory_ConcreteClass(int age, string name) => new(name, age);
    
    // Factories can also be async by wrapping the return type in a Task<T> …
    private async Task<decimal> DIE_Factory_RandomDecimal()
    {
        await Task.Yield();
        return 42.42m;
    }
    
    // … or a ValueTask<T>.
    private ValueTask<char> DIE_Factory_RandomChar => new('a');
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        await using var container = Container.DIE_CreateContainer();
        // For creation of following instances the container had to use the custom factories.
        var person = container.Create();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Name: Jane Doe, Age: 42
        var randomData = await container.CreateRandomData().ConfigureAwait(false);
        Console.WriteLine($"RandomDecimal: {randomData.RandomDecimal}, RandomChar: {randomData.RandomChar}"); // RandomDecimal: 42.42, RandomChar: a
    }
}