using System.Collections.Immutable;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.KeyedInjectionsAllKeysMultipleItems)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.KeyedInjections.AllKeysMultipleItems;

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

// This class will get a map from the key type to the interface type injected
internal class Parent
{
    // Not necessarily a map, but you can inject to an enumerable of an iterable of key-value-pairs as well
    internal IEnumerable<KeyValuePair<Key, IEnumerable<IInterface>>> Dependency0 { get; }
    internal required IReadOnlyDictionary<Key, IReadOnlyList<IInterface>> Dependency1 { get; init; }
    internal ImmutableDictionary<Key, ImmutableArray<IInterface>>? Dependency2 { get; private set; }
    
    internal Parent(IEnumerable<KeyValuePair<Key, IEnumerable<IInterface>>> dependency0) => Dependency0 = dependency0;

    internal void Initialize(ImmutableDictionary<Key, ImmutableArray<IInterface>> dependency2) => Dependency2 = dependency2;
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
        {
            var map = parent.Dependency0;
            var containsD = false;
            Console.WriteLine(map.Count()); // 3
            foreach (var (key, value) in map)
            {
                Console.WriteLine(key);
                foreach (var item in value)
                    Console.WriteLine(item.GetType().Name);
                if (key == Key.D) containsD = true;
            }
            // Because the key D is assigned to multiple implementations and a single value type is expected,
            // there won't be an entry for key D in the map.
            Console.WriteLine(containsD); // False
        }
        {
            var map = parent.Dependency1;
            Console.WriteLine(map.Count); // 3
            Check(Key.A);
            Check(Key.B);
            Check(Key.C);
            Check(Key.D);
            
            void Check(Key key)
            {
                Console.WriteLine(key);
                if (map.TryGetValue(key, out var list))
                    foreach (var item in list)
                        Console.WriteLine(item.GetType().Name);
                else Console.WriteLine($"No value for key {key}");
            }
        }
        {
            var map = parent.Dependency2!;
            Console.WriteLine(map.Count); // 3
            Check(Key.A);
            Check(Key.B);
            Check(Key.C);
            Check(Key.D);
            
            void Check(Key key)
            {
                Console.WriteLine(key);
                if (map.TryGetValue(key, out var list))
                    foreach (var item in list)
                        Console.WriteLine(item.GetType().Name);
                else Console.WriteLine($"No value for key {key}");
            }
        }
    }
}