using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomPropertyInjection)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomPropertyInjection;

internal class Person
{
    internal string Name { get; init; } = "";
    public int Age { get; init; }
}

[ImplementationAggregation(typeof(Person))]
[CreateFunction(typeof(Person), "Create")]
internal partial class Container
{
    private Container() {}
    
    private byte DIE_Factory_AgeHalf => 21;
    
    [UserDefinedPropertiesInjection(typeof(Person))]
    private void DIE_Props_Person(byte ageHalf, out string Name, out int Age)
    {
        Name = "Jane Doe";
        Age = ageHalf * 2;
    }
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        await using var container = Container.DIE_CreateContainer();
        var person = container.Create();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Name: Jane Doe, Age: 42
    }
}