using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomFactory)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomFactory;

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
    private int DIE_Factory_Age = 42;
    
    private Container() {}
    
    private string DIE_Factory_Name => "Jane Doe";
    
    private Person DIE_Factory_ConcreteClass() => new(DIE_Factory_Name, DIE_Factory_Age);
    
    private async Task<decimal> DIE_Factory_RandomDecimal()
    {
        await Task.Yield();
        return 42.42m;
    }
    
    private ValueTask<char> DIE_Factory_RandomChar => new('a');
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        await using var container = Container.DIE_CreateContainer();
        var person = container.Create();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Name: Jane Doe, Age: 42
        var randomData = await container.CreateRandomData().ConfigureAwait(false);
        Console.WriteLine($"RandomDecimal: {randomData.RandomDecimal}, RandomChar: {randomData.RandomChar}"); // RandomDecimal: 42.42, RandomChar: a
    }
}