using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomTypeInitializerInjection)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomCustomTypeInitializerInjection;

internal class Person
{
    internal void Initialize(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    internal string Name { get; private set; } = "";
    public int Age { get; private set; }
}

[ImplementationAggregation(typeof(Person))]
[Initializer(typeof(Person), nameof(Person.Initialize))]
[CreateFunction(typeof(Person), "Create")]
internal partial class Container
{
    private Container() {}
    
    private byte DIE_Factory_AgeHalf => 21;
    
    [UserDefinedInitializerParametersInjection(typeof(Person))]
    private void DIE_InitParams_Person(byte ageHalf, out string name, out int age)
    {
        name = "Jane Doe";
        age = ageHalf * 2;
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