using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.IterablesRemainingIterableTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Iterables.RemainingIterableTypes;

internal interface IInterface
{
}

internal class ConcreteClass : IInterface
{
}

internal class Struct : IInterface
{
}

internal record Record : IInterface;

internal record struct RecordStruct : IInterface;

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
        var container = Container.DIE_CreateContainer();
        var iCollection = container.CreateICollection(); // ConcreteClass, Struct, Record, RecordStruct
        var iReadOnlyCollection = container.CreateIReadOnlyCollection(); // ConcreteClass, Struct, Record, RecordStruct
        var readOnlyCollection = container.CreateReadOnlyCollection(); // ConcreteClass, Struct, Record, RecordStruct
        var arraySegment = container.CreateArraySegment(); // ConcreteClass, Struct, Record, RecordStruct
        var concurrentBag = container.CreateConcurrentBag(); // ConcreteClass, Struct, Record, RecordStruct
        var concurrentQueue = container.CreateConcurrentQueue(); // ConcreteClass, Struct, Record, RecordStruct
        var concurrentStack = container.CreateConcurrentStack(); // ConcreteClass, Struct, Record, RecordStruct
        var hashSet = container.CreateHashSet(); // ConcreteClass, Struct, Record, RecordStruct
        var linkedList = container.CreateLinkedList(); // ConcreteClass, Struct, Record, RecordStruct
        var queue = container.CreateQueue(); // ConcreteClass, Struct, Record, RecordStruct
        var stack = container.CreateStack(); // ConcreteClass, Struct, Record, RecordStruct
        var sortedSet = container.CreateSortedSet(); // ConcreteClass, Struct, Record, RecordStruct
        var immutableArray = container.CreateImmutableArray(); // ConcreteClass, Struct, Record, RecordStruct
        var immutableHashSet = container.CreateImmutableHashSet(); // ConcreteClass, Struct, Record, RecordStruct
        var immutableList = container.CreateImmutableList(); // ConcreteClass, Struct, Record, RecordStruct
        var immutableQueue = container.CreateImmutableQueue(); // ConcreteClass, Struct, Record, RecordStruct
        var immutableSortedSet = container.CreateImmutableSortedSet(); // ConcreteClass, Struct, Record, RecordStruct
        var immutableStack = container.CreateImmutableStack(); // ConcreteClass, Struct, Record, RecordStruct
        // Do something with implementation
    }
}