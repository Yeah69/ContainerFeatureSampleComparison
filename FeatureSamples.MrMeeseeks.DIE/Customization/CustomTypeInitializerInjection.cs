using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.CustomizationCustomTypeInitializerInjection)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Customization.CustomCustomTypeInitializerInjection;

// The dependencies of this class will be custom type initializer parameter injections.
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
    
    // Just a helpful custom factory.
    private byte DIE_Factory_AgeHalf => 21;
    
    // A method which has the name started with "DIE_InitParams" is needed for custom type initializer parameter injections.
    // Each type initializer parameter which should be injected in a custom way has to have an out-parameter with matching type and name of the original type initializer parameter.
    // Also the method requires following attribute in order to assign it to an implementation type.
    [UserDefinedInitializerParametersInjection(typeof(Person))]
    private void DIE_InitParams_Person(byte ageHalf, out string name, out int age)
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
        // The dependencies of the following instance will be injected by the custom type initializer parameter injection.
        var person = container.Create();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Name: Jane Doe, Age: 42
    }
}