using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.GenericsImplementation)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Generics.Implementation;

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

[ImplementationAggregation(typeof(ConcreteClass<>), typeof(Struct<>), typeof(Record<>), typeof(RecordStruct<>))]
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
        var container = Container.DIE_CreateContainer();
        var concreteClassOfInt = container.CreateConcreteClassOfInt();
        var concreteClassOfString = container.CreateConcreteClassOfString();
        var structOfInt = container.CreateStructOfInt();
        var structOfString = container.CreateStructOfString();
        var recordOfInt = container.CreateRecordOfInt();
        var recordOfString = container.CreateRecordOfString();
        var recordStructOfInt = container.CreateRecordStructOfInt();
        var recordStructOfString = container.CreateRecordStructOfString();
        // Do something with implementation
    }
}