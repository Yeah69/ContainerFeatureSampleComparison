using ContainerFeatureSampleComparison.FeatureDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:FeatureSample(Feature.KeyedInjectionsSupportedKeyTypes)]

namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection.KeyedInjections.SupportedKeyTypes;

// Simple enum that will be used as a key for the injections
internal enum Key
{
    A,
    B,
    C
}

// Simple interface that will work as an abstraction for the implementations
internal interface IInterface { }

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
    public Parent(
        [FromKeyedServices(Key.B)] IInterface dependencyEnum, 
        [FromKeyedServices(ConcreteClassB.Byte)] IInterface dependencyByte, 
        [FromKeyedServices(ConcreteClassB.SByte)] IInterface dependencySByte, 
        [FromKeyedServices(ConcreteClassB.Short)] IInterface dependencyShort,
        [FromKeyedServices(ConcreteClassB.UShort)] IInterface dependencyUShort, 
        [FromKeyedServices(ConcreteClassB.Int)] IInterface dependencyInt,
        [FromKeyedServices(ConcreteClassB.UInt)] IInterface dependencyUInt, 
        [FromKeyedServices(ConcreteClassB.Long)] IInterface dependencyLong, 
        [FromKeyedServices(ConcreteClassB.ULong)] IInterface dependencyULong,
        [FromKeyedServices(ConcreteClassB.Char)] IInterface dependencyChar,
        [FromKeyedServices(ConcreteClassB.Float)] IInterface dependencyFloat, 
        [FromKeyedServices(ConcreteClassB.Double)] IInterface dependencyDouble,
        [FromKeyedServices(ConcreteClassB.String)] IInterface dependencyString,
        [FromKeyedServices(ConcreteClassB.Bool)] IInterface dependencyBool,
        [FromKeyedServices(typeof(ConcreteClassB))] IInterface dependencyType)
    {
        DependencyEnum = dependencyEnum;
        DependencyByte = dependencyByte;
        DependencySByte = dependencySByte;
        DependencyShort = dependencyShort;
        DependencyUShort = dependencyUShort;
        DependencyInt = dependencyInt;
        DependencyUInt = dependencyUInt;
        DependencyLong = dependencyLong;
        DependencyULong = dependencyULong;
        DependencyChar = dependencyChar;
        DependencyFloat = dependencyFloat;
        DependencyDouble = dependencyDouble;
        DependencyString = dependencyString;
        DependencyBool = dependencyBool;
        DependencyType = dependencyType;
    }
    
    public required IInterface DependencyEnum { get; init; }
    
    public required IInterface DependencyByte { get; init; }
    
    public required IInterface DependencySByte { get; init; }
    
    public required IInterface DependencyShort { get; init; }
    
    public required IInterface DependencyUShort { get; init; }
    
    public required IInterface DependencyInt { get; init; }
    
    public required IInterface DependencyUInt { get; init; }
    
    public required IInterface DependencyLong { get; init; }
    
    public required IInterface DependencyULong { get; init; }
    
    public required IInterface DependencyChar { get; init; }
    
    public required IInterface DependencyFloat { get; init; }
    
    public required IInterface DependencyDouble { get; init; }
    
    public required IInterface DependencyString { get; init; }
    
    public required IInterface DependencyBool { get; init; }
    
    public required IInterface DependencyType { get; init; }
}

internal static class Builder
{
    internal static HostApplicationBuilder CreateBuilder(object anyObjectKeyA, object anyObjectKeyB, object anyObjectKeyC)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<Parent>();
        
        // Register the implementations with the appropriate keys
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(Key.A);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Byte);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.SByte);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Short);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.UShort);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Int);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.UInt);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Long);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.ULong);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Char);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Float);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Double);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.String);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(ConcreteClassA.Bool);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(typeof(ConcreteClassA));
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(Key.B);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Byte);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.SByte);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Short);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.UShort);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Int);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.UInt);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Long);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.ULong);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Char);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Float);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Double);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.String);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(ConcreteClassB.Bool);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(typeof(ConcreteClassB));
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(Key.C);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Byte);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.SByte);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Short);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.UShort);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Int);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.UInt);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Long);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.ULong);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Char);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Float);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Double);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.String);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(ConcreteClassC.Bool);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(typeof(ConcreteClassC));
        
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassA>(anyObjectKeyA);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassB>(anyObjectKeyB);
        builder.Services.AddKeyedTransient<IInterface, ConcreteClassC>(anyObjectKeyC);
        
        return builder;
    }
}

internal static class Usage
{
    internal static void Use()
    {
        var anyObjectKeyA = new object();
        var anyObjectKeyB = new object();
        var anyObjectKeyC = new object();
        using var host = Builder.CreateBuilder(anyObjectKeyA, anyObjectKeyB, anyObjectKeyC).Build();

        var parent = host.Services.GetRequiredService<Parent>();
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

        var instanceByAnyObject = host.Services.GetRequiredKeyedService<IInterface>(anyObjectKeyB);
        Console.WriteLine(instanceByAnyObject.GetType().Name); // ConcreteClassB
    }
}