using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.KeyedInjectionsSupportedKeyTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.KeyedInjections.SupportedKeyTypes;

// Simple enum that will be used as a key for the injections
internal enum Key
{
    A,
    B,
    C
}

// Simple attribute that will be used to configure a key directly on an implementation type
// or assign a key to a parameter or property.
// The attribute type requires to have a constructor that has its first parameter of type object.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = true)]
internal class KeyAssignmentAttribute : Attribute
{
    internal KeyAssignmentAttribute(object value) {}
}

// Simple interface that will work as an abstraction for the implementations
internal interface IInterface
{
}

[KeyAssignment(Key.A)]
[KeyAssignment(Byte)]
[KeyAssignment(SByte)]
[KeyAssignment(Short)]
[KeyAssignment(UShort)]
[KeyAssignment(Int)]
[KeyAssignment(UInt)]
[KeyAssignment(Long)]
[KeyAssignment(ULong)]
[KeyAssignment(Char)]
[KeyAssignment(Float)]
[KeyAssignment(Double)]
[KeyAssignment(String)]
[KeyAssignment(Bool)]
[KeyAssignment(typeof(ConcreteClassA))]
internal class ConcreteClassA : IInterface
{
    internal const byte Byte = 0;
    internal const sbyte SByte = 1;
    internal const short Short = 2;
    internal const ushort UShort = 3;
    internal const int Int = 4;
    internal const uint UInt = 5;
    internal const long Long = 6;
    internal const ulong ULong = 7;
    internal const char Char = (char) 8;
    internal const float Float = 9.0f;
    internal const double Double = 10.0;
    internal const string String = "11";
    internal const bool Bool = true;
}

[KeyAssignment(Key.B)]
[KeyAssignment(Byte)]
[KeyAssignment(SByte)]
[KeyAssignment(Short)]
[KeyAssignment(UShort)]
[KeyAssignment(Int)]
[KeyAssignment(UInt)]
[KeyAssignment(Long)]
[KeyAssignment(ULong)]
[KeyAssignment(Char)]
[KeyAssignment(Float)]
[KeyAssignment(Double)]
[KeyAssignment(String)]
[KeyAssignment(Bool)]
[KeyAssignment(typeof(ConcreteClassB))]
internal class ConcreteClassB : IInterface
{
    internal const byte Byte = 12;
    internal const sbyte SByte = 13;
    internal const short Short = 14;
    internal const ushort UShort = 15;
    internal const int Int = 16;
    internal const uint UInt = 17;
    internal const long Long = 18;
    internal const ulong ULong = 19;
    internal const char Char = (char) 20;
    internal const float Float = 21.0f;
    internal const double Double = 22.0;
    internal const string String = "23";
    internal const bool Bool = false;
}


[KeyAssignment(Key.C)]
[KeyAssignment(Byte)]
[KeyAssignment(SByte)]
[KeyAssignment(Short)]
[KeyAssignment(UShort)]
[KeyAssignment(Int)]
[KeyAssignment(UInt)]
[KeyAssignment(Long)]
[KeyAssignment(ULong)]
[KeyAssignment(Char)]
[KeyAssignment(Float)]
[KeyAssignment(Double)]
[KeyAssignment(String)]
[KeyAssignment(Bool)]
[KeyAssignment(typeof(ConcreteClassC))]
internal class ConcreteClassC : IInterface
{
    internal const byte Byte = 24;
    internal const sbyte SByte = 25;
    internal const short Short = 26;
    internal const ushort UShort = 27;
    internal const int Int = 28;
    internal const uint UInt = 29;
    internal const long Long = 30;
    internal const ulong ULong = 31;
    internal const char Char = (char) 32;
    internal const float Float = 33.0f;
    internal const double Double = 34.0;
    internal const string String = "35";
    internal const bool Bool = true;
}

internal class Parent
{
    [KeyAssignment(Key.B)]
    public required IInterface DependencyEnum { get; init; }
    
    [KeyAssignment(ConcreteClassB.Byte)]
    public required IInterface DependencyByte { get; init; }
    
    [KeyAssignment(ConcreteClassB.SByte)]
    public required IInterface DependencySByte { get; init; }

    [KeyAssignment(ConcreteClassB.Short)]
    public required IInterface DependencyShort { get; init; }

    [KeyAssignment(ConcreteClassB.UShort)]
    public required IInterface DependencyUShort { get; init; }

    [KeyAssignment(ConcreteClassB.Int)]
    public required IInterface DependencyInt { get; init; }

    [KeyAssignment(ConcreteClassB.UInt)]
    public required IInterface DependencyUInt { get; init; }

    [KeyAssignment(ConcreteClassB.Long)]
    public required IInterface DependencyLong { get; init; }

    [KeyAssignment(ConcreteClassB.ULong)]
    public required IInterface DependencyULong { get; init; }

    [KeyAssignment(ConcreteClassB.Char)]
    public required IInterface DependencyChar { get; init; }

    [KeyAssignment(ConcreteClassB.Float)]
    public required IInterface DependencyFloat { get; init; }

    [KeyAssignment(ConcreteClassB.Double)]
    public required IInterface DependencyDouble { get; init; }

    [KeyAssignment(ConcreteClassB.String)]
    public required IInterface DependencyString { get; init; }

    [KeyAssignment(ConcreteClassB.Bool)]
    public required IInterface DependencyBool { get; init; }

    [KeyAssignment(typeof(ConcreteClassB))]
    public required IInterface DependencyType { get; init; }
}

// Registering all the implementations
[ImplementationAggregation(
    typeof(Parent),
    typeof(ConcreteClassA), 
    typeof(ConcreteClassB), 
    typeof(ConcreteClassC))]

// Configure a mapping to the custom key assigning attribute
[InjectionKeyMapping(typeof(KeyAssignmentAttribute))]

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
        Console.WriteLine(parent.DependencyEnum.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyByte.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencySByte.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyShort.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyUShort.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyInt.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyUInt.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyLong.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyULong.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyChar.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyFloat.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyDouble.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyString.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyBool.GetType().Name); // ConcreteClassB
        Console.WriteLine(parent.DependencyType.GetType().Name); // ConcreteClassB
    }
}