using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.KeyedInjectionsOneKeySingleItem)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.KeyedInjections.OneKeySingleItem;

// Simple enum that will be used as a key for the injections
internal enum Key
{
    A,
    B,
    C,
    D
}

// Simple attribute that will be used to configure a key directly on an implementation type
// or assign a key to a parameter or property.
// The attribute type requires to have a constructor that has its first parameter of type object.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.Property)]
internal class KeyAssignmentAttribute : Attribute
{
    internal KeyAssignmentAttribute(object value) {}
}

// Simple interface that will work as an abstraction for the implementations
internal interface IInterface {}

// ConcreteClassA gets the key A assigned via the attribute
[KeyAssignment(Key.A)]
internal class ConcreteClassA : IInterface {}

// ConcreteClassB will get the key assigned via the container's configuration (see below)
internal class ConcreteClassB : IInterface {}

// ConcreteClassC gets the key C assigned via the attribute
[KeyAssignment(Key.C)]
internal class ConcreteClassC : IInterface {}

// The next two implementations will get the key D assigned via the attribute
[KeyAssignment(Key.D)]
internal class ConcreteClassD0 : IInterface {}
[KeyAssignment(Key.D)]
internal class ConcreteClassD1 : IInterface {}

// This class gets multiple instances of IInterface injected distinguished by keys
internal class Parent
{
    internal IInterface DependencyA { get; }
    // This property will be injected directly by property injection. Hence, it gets the key B assigned
    [KeyAssignment(Key.B)]
    internal required IInterface DependencyB { get; init; }
    internal IInterface? DependencyC { get; private set; }
    // Because the key D is assigned to multiple implementations and because the property type is nullable,
    // the property will get injected with null instead of one of the implementations.
    // If the property type was not nullable, a compile error would occur.
    [KeyAssignment(Key.D)]
    internal required IInterface? DependencyD { get; init; }
    
    // To make a constructor parameter a keyed injection just assign the key via the attribute
    internal Parent([KeyAssignment(Key.A)] IInterface dependencyA) => DependencyA = dependencyA;

    // In the same manner, parameter of initializer method can be keyed as well
    internal void Initialize([KeyAssignment(Key.C)] IInterface dependencyC) => DependencyC = dependencyC;
}

// Registering all the implementations
[ImplementationAggregation(
    typeof(Parent),
    typeof(ConcreteClassA), 
    typeof(ConcreteClassB), 
    typeof(ConcreteClassC), 
    typeof(ConcreteClassD0), 
    typeof(ConcreteClassD1))]
[Initializer(typeof(Parent), nameof(Parent.Initialize))]

// Configure a mapping to the custom key assigning attribute
[InjectionKeyMapping(typeof(KeyAssignmentAttribute))]
// Register the key B to the implementation ConcreteClassB explicitly
// The injection key choice is convenient for configuring keys types from external libraries,
// where you can't append the custom key assigning attribute to the implementation type.
[InjectionKeyChoice(Key.B, typeof(ConcreteClassB))]

[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var parent = container.Create();
        Console.WriteLine(parent.DependencyA.GetType().Name); // ConcreteClassA
        Console.WriteLine(parent.DependencyB.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyC!.GetType().Name); // ConcreteClassC
        Console.WriteLine(parent.DependencyD is null); // True
    }
}