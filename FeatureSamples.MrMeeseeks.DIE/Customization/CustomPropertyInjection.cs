using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomPropertyInjection)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomPropertyInjection;

// The dependencies of this class will be custom property injections.
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
    
    // Just a helpful custom factory.
    private byte DIE_Factory_AgeHalf => 21;
    
    // A method which has the name started with "DIE_Props" is needed for custom property injections.
    // Each property which should be injected in a custom way has to have an out-parameter with matching type and name of the original property.
    // Also the method requires following attribute in order to assign it to an implementation type.
    [UserDefinedPropertiesInjection(typeof(Person))]
    private void DIE_Props_Person(byte ageHalf, out string Name, out int Age)
    {
        // Set the out-parameters to the values you want to inject.
        Name = "Jane Doe";
        Age = ageHalf * 2;
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