using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.GenericsImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Generics.Implementation;

// Different types of generic implementations
internal class ConcreteClass<T>
{
}

internal struct Struct<T>
{
}

internal record Record<T>
{
}

internal record struct RecordStruct<T>
{
}

// Register the generic implementations
[ImplementationAggregation(typeof(ConcreteClass<>), typeof(Struct<>), typeof(Record<>), typeof(RecordStruct<>))]
// Define create functions for some closed versions of the generic implementation types
[CreateFunction(typeof(ConcreteClass<int>), "CreateConcreteClassOfInt")]
[CreateFunction(typeof(ConcreteClass<string>), "CreateConcreteClassOfString")]
[CreateFunction(typeof(Struct<int>), "CreateStructOfInt")]
[CreateFunction(typeof(Struct<string>), "CreateStructOfString")]
[CreateFunction(typeof(Record<int>), "CreateRecordOfInt")]
[CreateFunction(typeof(Record<string>), "CreateRecordOfString")]
[CreateFunction(typeof(RecordStruct<int>), "CreateRecordStructOfInt")]
[CreateFunction(typeof(RecordStruct<string>), "CreateRecordStructOfString")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClassOfInt = container.CreateConcreteClassOfInt();
        var concreteClassOfString = container.CreateConcreteClassOfString();
        var structOfInt = container.CreateStructOfInt();
        var structOfString = container.CreateStructOfString();
        var recordOfInt = container.CreateRecordOfInt();
        var recordOfString = container.CreateRecordOfString();
        var recordStructOfInt = container.CreateRecordStructOfInt();
        var recordStructOfString = container.CreateRecordStructOfString();
        Console.WriteLine($"{concreteClassOfInt.GetType().Name} {concreteClassOfInt.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 Int32
        Console.WriteLine($"{concreteClassOfString.GetType().Name} {concreteClassOfString.GetType().GenericTypeArguments.First().Name}"); // ConcreteClass`1 String
        Console.WriteLine($"{structOfInt.GetType().Name} {structOfInt.GetType().GenericTypeArguments.First().Name}"); // Struct`1 Int32
        Console.WriteLine($"{structOfString.GetType().Name} {structOfString.GetType().GenericTypeArguments.First().Name}"); // Struct`1 String
        Console.WriteLine($"{recordOfInt.GetType().Name} {recordOfInt.GetType().GenericTypeArguments.First().Name}"); // Record`1 Int32
        Console.WriteLine($"{recordOfString.GetType().Name} {recordOfString.GetType().GenericTypeArguments.First().Name}"); // Record`1 String
        Console.WriteLine($"{recordStructOfInt.GetType().Name} {recordStructOfInt.GetType().GenericTypeArguments.First().Name}"); // RecordStruct`1 Int32
        Console.WriteLine($"{recordStructOfString.GetType().Name} {recordStructOfString.GetType().GenericTypeArguments.First().Name}"); // RecordStruct`1 String
    }
}