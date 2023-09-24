using System.Collections.Immutable;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.KeyedInjectionsAllKeysSingleItem)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.KeyedInjections.AllKeysSingleItem;

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

// This class will get a map from the key type to the interface type injected
internal class Parent
{
    // Not necessarily a map, but you can inject to an enumerable of key-value-pairs as well
    internal IEnumerable<KeyValuePair<Key, IInterface>> Dependency0 { get; }
    internal required IReadOnlyDictionary<Key, IInterface> Dependency1 { get; init; }
    internal ImmutableSortedDictionary<Key, IInterface>? Dependency2 { get; private set; }
    
    internal Parent(IEnumerable<KeyValuePair<Key, IInterface>> dependency0) => Dependency0 = dependency0;

    internal void Initialize(ImmutableSortedDictionary<Key, IInterface> dependency2) => Dependency2 = dependency2;
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
        Check(parent.Dependency0);
        Check(parent.Dependency1);
        Check(parent.Dependency2!);
        return;

        void Check(IEnumerable<KeyValuePair<Key, IInterface>> map)
        {
            var containsD = false;
            Console.WriteLine(map.Count()); // 3
            foreach (var (key, value) in map)
            {
                Console.WriteLine(key);
                Console.WriteLine(value.GetType().Name);
                if (key == Key.D) containsD = true;
            }
            // Because the key D is assigned to multiple implementations and a single value type is expected,
            // there won't be an entry for key D in the map.
            Console.WriteLine(containsD); // False
        }
    }
}