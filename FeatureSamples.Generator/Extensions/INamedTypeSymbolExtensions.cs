using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContainerFeatureSampleComparison.FeatureSamples.Generator.Extensions;

public static class INamedTypeSymbolExtensions
{
    public static IEnumerable<INamedTypeSymbol> AllBaseTypesAndSelf(this INamedTypeSymbol type)
    {
        if (type.TypeKind is not (TypeKind.Class or TypeKind.Struct)) 
            yield break;

        yield return type;
        foreach (var baseType in type.AllBaseTypes())
            yield return baseType;
    }
    public static IEnumerable<INamedTypeSymbol> AllBaseTypes(this INamedTypeSymbol type)
    {
        if (type.TypeKind is not (TypeKind.Class or TypeKind.Struct)) 
            yield break;
        
        var temp = type.BaseType;
        while (temp is {})
        {
            yield return temp;
            temp = temp.BaseType;
        }
    }
    public static IEnumerable<INamedTypeSymbol> AllDerivedTypesAndSelf(this INamedTypeSymbol type)
    {
        yield return type;
        
        foreach (var derivedType in type.AllDerivedTypes())
            yield return derivedType;
    }
    public static IEnumerable<INamedTypeSymbol> AllDerivedTypes(this INamedTypeSymbol type)
    {
        foreach (var interfaceType in type
                     .AllInterfaces)
            yield return interfaceType;
        if (type.TypeKind is TypeKind.Class or TypeKind.Struct)
        {
            var temp = type.BaseType;
            while (temp is {})
            {
                yield return temp;
                temp = temp.BaseType;
            }
        }
    }
    public static IEnumerable<INamedTypeSymbol> AllNestedTypesAndSelf(this INamedTypeSymbol type)
    {
        yield return type;
        foreach (var nestedType in type.AllNestedTypes())
        {
            yield return nestedType;
        }
    }
    public static IEnumerable<INamedTypeSymbol> AllNestedTypes(this INamedTypeSymbol type)
    {
        foreach (var typeMember in type.GetTypeMembers())
        {
            foreach (var nestedType in typeMember.AllNestedTypesAndSelf())
            {
                yield return nestedType;
            }
        }
    }
    
    public static INamedTypeSymbol UnboundIfGeneric(this INamedTypeSymbol type) =>
        type is { IsGenericType: true, IsUnboundGenericType: false }
            ? type.ConstructUnboundGenericType()
            : type;
    
    public static INamedTypeSymbol OriginalDefinitionIfUnbound(this INamedTypeSymbol type) =>
        type.IsUnboundGenericType
            ? type.OriginalDefinition
            : type;
    
    public static IEnumerable<TypeDeclarationSyntax> GetTypeDeclarationSyntax(this INamedTypeSymbol type) =>
        type.DeclaringSyntaxReferences
            .Select(declaringSyntaxReference => declaringSyntaxReference.GetSyntax())
            .OfType<TypeDeclarationSyntax>();
    
    public static bool IsPartial(this INamedTypeSymbol type) =>
        type.GetTypeDeclarationSyntax()
            .All(typeDeclarationSyntax => typeDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword));
}