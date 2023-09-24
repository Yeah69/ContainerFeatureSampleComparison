using System.Collections.Immutable;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.KeyedInjectionsOneKeyMultipleItems)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.KeyedInjections.OneKeyMultipleItems;

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

// ConcreteClassA0 & ConcreteClassA1 get the key A assigned via the attribute
[KeyAssignment(Key.A)]
internal class ConcreteClassA0 : IInterface {}
[KeyAssignment(Key.A)]
internal class ConcreteClassA1 : IInterface {}

// ConcreteClassB0 and ConcreteClassB1 will get the key assigned via the container's configuration (see below)
internal class ConcreteClassB0 : IInterface {}
internal class ConcreteClassB1 : IInterface {}

// ConcreteClassC0 & ConcreteClassC1 gets the key C assigned via the attribute
[KeyAssignment(Key.C)]
internal class ConcreteClassC0 : IInterface {}
[KeyAssignment(Key.C)]
internal class ConcreteClassC1 : IInterface {}

// This class gets multiple instances of IInterface injected distinguished by keys
internal class Parent
{
    internal IEnumerable<IInterface> DependencyA { get; }
    // This property will be injected directly by property injection. Hence, it gets the key B assigned
    [KeyAssignment(Key.B)]
    internal required IReadOnlyList<IInterface> DependencyB { get; init; }
    internal List<IInterface>? DependencyC { get; private set; }
    [KeyAssignment(Key.D)]
    internal required ImmutableArray<IInterface> DependencyD { get; init; }
    
    // To make a constructor parameter a keyed injection just assign the key via the attribute
    internal Parent([KeyAssignment(Key.A)] IEnumerable<IInterface> dependencyA) => DependencyA = dependencyA;

    // In the same manner, parameter of initializer method can be keyed as well
    internal void Initialize([KeyAssignment(Key.C)] List<IInterface> dependencyC) => DependencyC = dependencyC;
}

// Registering all the implementations
[ImplementationAggregation(
    typeof(Parent),
    typeof(ConcreteClassA0), 
    typeof(ConcreteClassA1), 
    typeof(ConcreteClassB0),  
    typeof(ConcreteClassB1),
    typeof(ConcreteClassC0),
    typeof(ConcreteClassC1))]
[Initializer(typeof(Parent), nameof(Parent.Initialize))]

// Configure a mapping to the custom key assigning attribute
[InjectionKeyMapping(typeof(KeyAssignmentAttribute))]
// Register the key B to the implementations ConcreteClassB0 & ConcreteClassB1 explicitly
// The injection key choice is convenient for configuring keys types from external libraries,
// where you can't append the custom key assigning attribute to the implementation type.
[InjectionKeyChoice(Key.B, typeof(ConcreteClassB0))]
[InjectionKeyChoice(Key.B, typeof(ConcreteClassB1))]

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
        Console.WriteLine(parent.DependencyA.Count()); // 2
        foreach (var a in parent.DependencyA)
            Console.WriteLine(a is ConcreteClassA0 or ConcreteClassA1); // True
        Console.WriteLine(parent.DependencyB.Count); // 2
        foreach (var b in parent.DependencyB)
            Console.WriteLine(b is ConcreteClassB0 or ConcreteClassB1); // True
        Console.WriteLine(parent.DependencyC!.Count); // 2
        foreach (var c in parent.DependencyC!)
            Console.WriteLine(c is ConcreteClassC0 or ConcreteClassC1); // True
        Console.WriteLine(parent.DependencyD.Length); // 0
    }
}