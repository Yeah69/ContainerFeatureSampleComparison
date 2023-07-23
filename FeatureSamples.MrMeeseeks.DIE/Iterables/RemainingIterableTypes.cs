using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.IterablesRemainingIterableTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Iterables.RemainingIterableTypes;

// An interface and its implementations
internal interface IInterface
{
}

internal class ConcreteClass : IInterface
{
}

internal struct Struct : IInterface
{
}

internal record Record : IInterface;

internal record struct RecordStruct : IInterface;

// Register the implementations
[ImplementationAggregation(typeof(ConcreteClass), typeof(Struct), typeof(Record), typeof(RecordStruct))]
[CreateFunction(typeof(ICollection<IInterface>), "CreateICollection")]
[CreateFunction(typeof(IReadOnlyCollection<IInterface>), "CreateIReadOnlyCollection")]
[CreateFunction(typeof(ReadOnlyCollection<IInterface>), "CreateReadOnlyCollection")]
[CreateFunction(typeof(ArraySegment<IInterface>), "CreateArraySegment")]
[CreateFunction(typeof(ConcurrentBag<IInterface>), "CreateConcurrentBag")]
[CreateFunction(typeof(ConcurrentQueue<IInterface>), "CreateConcurrentQueue")]
[CreateFunction(typeof(ConcurrentStack<IInterface>), "CreateConcurrentStack")]
[CreateFunction(typeof(HashSet<IInterface>), "CreateHashSet")]
[CreateFunction(typeof(LinkedList<IInterface>), "CreateLinkedList")]
[CreateFunction(typeof(Queue<IInterface>), "CreateQueue")]
[CreateFunction(typeof(Stack<IInterface>), "CreateStack")]
[CreateFunction(typeof(SortedSet<IInterface>), "CreateSortedSet")]
[CreateFunction(typeof(ImmutableArray<IInterface>), "CreateImmutableArray")]
[CreateFunction(typeof(ImmutableHashSet<IInterface>), "CreateImmutableHashSet")]
[CreateFunction(typeof(ImmutableList<IInterface>), "CreateImmutableList")]
[CreateFunction(typeof(ImmutableQueue<IInterface>), "CreateImmutableQueue")]
[CreateFunction(typeof(ImmutableSortedSet<IInterface>), "CreateImmutableSortedSet")]
[CreateFunction(typeof(ImmutableStack<IInterface>), "CreateImmutableStack")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var iCollection = container.CreateICollection();
        foreach (var implementation in iCollection)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var iReadOnlyCollection = container.CreateIReadOnlyCollection();
        foreach (var implementation in iReadOnlyCollection)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var readOnlyCollection = container.CreateReadOnlyCollection();
        foreach (var implementation in readOnlyCollection)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var arraySegment = container.CreateArraySegment();
        foreach (var implementation in arraySegment)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var concurrentBag = container.CreateConcurrentBag();
        foreach (var implementation in concurrentBag)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var concurrentQueue = container.CreateConcurrentQueue();
        foreach (var implementation in concurrentQueue)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var concurrentStack = container.CreateConcurrentStack();
        foreach (var implementation in concurrentStack)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var hashSet = container.CreateHashSet();
        foreach (var implementation in hashSet)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var linkedList = container.CreateLinkedList();
        foreach (var implementation in linkedList)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var queue = container.CreateQueue();
        foreach (var implementation in queue)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var stack = container.CreateStack();
        foreach (var implementation in stack)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var sortedSet = container.CreateSortedSet();
        foreach (var implementation in sortedSet)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var immutableArray = container.CreateImmutableArray();
        foreach (var implementation in immutableArray)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var immutableHashSet = container.CreateImmutableHashSet();
        foreach (var implementation in immutableHashSet)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var immutableList = container.CreateImmutableList();
        foreach (var implementation in immutableList)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var immutableQueue = container.CreateImmutableQueue();
        foreach (var implementation in immutableQueue)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var immutableSortedSet = container.CreateImmutableSortedSet();
        foreach (var implementation in immutableSortedSet)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
        var immutableStack = container.CreateImmutableStack();
        foreach (var implementation in immutableStack)
        {
            Console.WriteLine(implementation.GetType().Name);
        }
    }
}