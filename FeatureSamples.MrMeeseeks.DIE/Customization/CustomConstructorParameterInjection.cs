using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomConstructorParameterInjection)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomConstructorParameterInjection;

// The dependencies of this class will be custom constructor parameter injections.
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

[ImplementationAggregation(typeof(Person))]
[CreateFunction(typeof(Person), "Create")]
internal partial class Container
{
    private Container() {}
    
    // Just a helpful custom factory.
    private byte DIE_Factory_AgeHalf => 21;
    
    // A method which has the name started with "DIE_ConstrParams" is needed for custom constructor parameter injections.
    // Each constructor parameter which should be injected in a custom way has to have an out-parameter with matching type and name of the original constructor parameter.
    // Also the method requires following attribute in order to assign it to an implementation type.
    [UserDefinedConstructorParametersInjection(typeof(Person))]
    private void DIE_ConstrParams_Person(byte ageHalf, out string name, out int age)
    {
        // Set the out-parameters to the values you want to inject.
        name = "Jane Doe";
        age = ageHalf * 2;
    }
}

internal static class Usage
{
    internal static async ValueTask Use()
    {
        await using var container = Container.DIE_CreateContainer();
        // The dependencies of the following instance will be injected by the custom constructor parameter injection.
        var person = container.Create();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Name: Jane Doe, Age: 42
    }
}