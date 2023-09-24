using System.Collections.Immutable;
using System.Collections.ObjectModel;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.KeyedInjectionsSupportedMapTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.KeyedInjections.SupportedMapTypes;

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
internal interface IInterface
{
}

[KeyAssignment(Key.A)]
internal class ConcreteClassA : IInterface { }

[KeyAssignment(Key.B)]
internal class ConcreteClassB : IInterface { }

[KeyAssignment(Key.C)]
internal class ConcreteClassC : IInterface { }

internal interface IAsyncInterface
{
}

[KeyAssignment(Key.A)]
internal class AsyncConcreteClassA : IAsyncInterface
{
    public Task InitializeAsync() => Task.CompletedTask;
}

[KeyAssignment(Key.B)]
internal class AsyncConcreteClassB : IAsyncInterface
{
    public async ValueTask InitializeAsync() => await Task.Yield();
}

[KeyAssignment(Key.C)]
internal class AsyncConcreteClassC : IAsyncInterface
{
    public Task InitializeAsync() => Task.CompletedTask;
}

internal class Parent
{
    public required IEnumerable<KeyValuePair<Key, IInterface>> Dependency { get; init; }
    public required IAsyncEnumerable<KeyValuePair<Key, IAsyncInterface>> DependencyIAsyncEnumerable { get; init; }
    public required ValueTask<IEnumerable<KeyValuePair<Key, IInterface>>> DependencyValueTaskIEnumerable { get; init; }
    public required Task<IEnumerable<KeyValuePair<Key, IInterface>>> DependencyTaskIEnumerable { get; init; }
    public required IEnumerable<KeyValuePair<Key, IAsyncInterface>> DependencyAsyncIEnumerable { get; init; }
    public required IDictionary<Key, IInterface> DependencyIDictionary { get; init; }
    public required IReadOnlyDictionary<Key, IInterface> DependencyIReadOnlyDictionary { get; init; }
    public required Dictionary<Key, IInterface> DependencyDictionary { get; init; }
    public required ReadOnlyDictionary<Key, IInterface> DependencyReadOnlyDictionary { get; init; }
    public required SortedDictionary<Key, IInterface> DependencySortedDictionary { get; init; }
    public required SortedList<Key, IInterface> DependencySortedList { get; init; }
    public required ImmutableDictionary<Key, IInterface> DependencyImmutableDictionary { get; init; }
    public required ImmutableSortedDictionary<Key, IInterface> DependencyImmutableSortedDictionary { get; init; }
    public required ValueTask<IDictionary<Key, IInterface>> DependencyValueTaskIDictionary { get; init; }
    public required ValueTask<IReadOnlyDictionary<Key, IInterface>> DependencyValueTaskIReadOnlyDictionary { get; init; }
    public required ValueTask<Dictionary<Key, IInterface>> DependencyValueTaskDictionary { get; init; }
    public required ValueTask<ReadOnlyDictionary<Key, IInterface>> DependencyValueTaskReadOnlyDictionary { get; init; }
    public required ValueTask<SortedDictionary<Key, IInterface>> DependencyValueTaskSortedDictionary { get; init; }
    public required ValueTask<SortedList<Key, IInterface>> DependencyValueTaskSortedList { get; init; }
    public required ValueTask<ImmutableDictionary<Key, IInterface>> DependencyValueTaskImmutableDictionary { get; init; }
    public required ValueTask<ImmutableSortedDictionary<Key, IInterface>> DependencyValueTaskImmutableSortedDictionary { get; init; }
    public required Task<IDictionary<Key, IInterface>> DependencyTaskIDictionary { get; init; }
    public required Task<IReadOnlyDictionary<Key, IInterface>> DependencyTaskIReadOnlyDictionary { get; init; }
    public required Task<Dictionary<Key, IInterface>> DependencyTaskDictionary { get; init; }
    public required Task<ReadOnlyDictionary<Key, IInterface>> DependencyTaskReadOnlyDictionary { get; init; }
    public required Task<SortedDictionary<Key, IInterface>> DependencyTaskSortedDictionary { get; init; }
    public required Task<SortedList<Key, IInterface>> DependencyTaskSortedList { get; init; }
    public required Task<ImmutableDictionary<Key, IInterface>> DependencyTaskImmutableDictionary { get; init; }
    public required Task<ImmutableSortedDictionary<Key, IInterface>> DependencyTaskImmutableSortedDictionary { get; init; }
    public required IDictionary<Key, IAsyncInterface> DependencyAsyncIDictionary { get; init; }
    public required IReadOnlyDictionary<Key, IAsyncInterface> DependencyAsyncIReadOnlyDictionary { get; init; }
    public required Dictionary<Key, IAsyncInterface> DependencyAsyncDictionary { get; init; }
    public required ReadOnlyDictionary<Key, IAsyncInterface> DependencyAsyncReadOnlyDictionary { get; init; }
    public required SortedDictionary<Key, IAsyncInterface> DependencyAsyncSortedDictionary { get; init; }
    public required SortedList<Key, IAsyncInterface> DependencyAsyncSortedList { get; init; }
    public required ImmutableDictionary<Key, IAsyncInterface> DependencyAsyncImmutableDictionary { get; init; }
    public required ImmutableSortedDictionary<Key, IAsyncInterface> DependencyAsyncImmutableSortedDictionary { get; init; }
}

// Registering all the implementations
[ImplementationAggregation(
    typeof(Parent),
    typeof(ConcreteClassA), 
    typeof(ConcreteClassB), 
    typeof(ConcreteClassC), 
    typeof(AsyncConcreteClassA), 
    typeof(AsyncConcreteClassB), 
    typeof(AsyncConcreteClassC))]
[Initializer(typeof(AsyncConcreteClassA), nameof(AsyncConcreteClassA.InitializeAsync))]
[Initializer(typeof(AsyncConcreteClassB), nameof(AsyncConcreteClassB.InitializeAsync))]
[Initializer(typeof(AsyncConcreteClassC), nameof(AsyncConcreteClassC.InitializeAsync))]

// Configure a mapping to the custom key assigning attribute
[InjectionKeyMapping(typeof(KeyAssignmentAttribute))]

[CreateFunction(typeof(Parent), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static async Task Use()
    {
        await using var container = Container.DIE_CreateContainer();
        var parent = await container.Create();
        Check(parent.Dependency);
        await CheckAsync(parent.DependencyIAsyncEnumerable);
        Check(await parent.DependencyValueTaskIEnumerable);
        Check(await parent.DependencyTaskIEnumerable);
        CheckIAsyncInterface(parent.DependencyAsyncIEnumerable);
        Check(parent.DependencyIDictionary);
        Check(parent.DependencyIReadOnlyDictionary);
        Check(parent.DependencyDictionary);
        Check(parent.DependencyReadOnlyDictionary);
        Check(parent.DependencySortedDictionary);
        Check(parent.DependencySortedList);
        Check(parent.DependencyImmutableDictionary);
        Check(parent.DependencyImmutableSortedDictionary);
        Check(await parent.DependencyValueTaskIDictionary);
        Check(await parent.DependencyValueTaskIReadOnlyDictionary);
        Check(await parent.DependencyValueTaskDictionary);
        Check(await parent.DependencyValueTaskReadOnlyDictionary);
        Check(await parent.DependencyValueTaskSortedDictionary);
        Check(await parent.DependencyValueTaskSortedList);
        Check(await parent.DependencyValueTaskImmutableDictionary);
        Check(await parent.DependencyValueTaskImmutableSortedDictionary);
        Check(await parent.DependencyTaskIDictionary);
        Check(await parent.DependencyTaskIReadOnlyDictionary);
        Check(await parent.DependencyTaskDictionary);
        Check(await parent.DependencyTaskReadOnlyDictionary);
        Check(await parent.DependencyTaskSortedDictionary);
        Check(await parent.DependencyTaskSortedList);
        Check(await parent.DependencyTaskImmutableDictionary);
        Check(await parent.DependencyTaskImmutableSortedDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncIDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncIReadOnlyDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncReadOnlyDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncSortedDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncSortedList);
        CheckIAsyncInterface(parent.DependencyAsyncImmutableDictionary);
        CheckIAsyncInterface(parent.DependencyAsyncImmutableSortedDictionary);
        return;

        void Check(IEnumerable<KeyValuePair<Key, IInterface>> map)
        {
            Console.WriteLine(map.GetType().Name);
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

        void CheckIAsyncInterface(IEnumerable<KeyValuePair<Key, IAsyncInterface>> map)
        {
            Console.WriteLine(map.GetType().Name);
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

        async Task CheckAsync(IAsyncEnumerable<KeyValuePair<Key, IAsyncInterface>> map)
        {
            Console.WriteLine(map.GetType().Name);
            var containsD = false;
            var count = 0;
            await foreach (var (key, value) in map)
            {
                count++;
                Console.WriteLine(key);
                Console.WriteLine(value.GetType().Name);
                if (key == Key.D) containsD = true;
            }
            Console.WriteLine(count); // 3
            // Because the key D is assigned to multiple implementations and a single value type is expected,
            // there won't be an entry for key D in the map.
            Console.WriteLine(containsD); // False
        }
    }
}