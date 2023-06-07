using System.Linq;
using Microsoft.CodeAnalysis;

namespace ContainerFeatureSampleComparison.FeatureSamples.Generator.Extensions;

// ReSharper disable once InconsistentNaming
public static class ITypeSymbolExtensions
{
    // Picked from https://github.com/YairHalberstadt/stronginject Thank you!
    public static bool IsOrReferencesErrorType(this ITypeSymbol type)
    {
        if (!type.ContainingType?.IsOrReferencesErrorType() ?? false)
            return false;
        return type switch
        {
            IErrorTypeSymbol => true,
            IArrayTypeSymbol array => array.ElementType.IsOrReferencesErrorType(),
            IPointerTypeSymbol pointer => pointer.PointedAtType.IsOrReferencesErrorType(),
            INamedTypeSymbol named => !named.IsUnboundGenericType && named.TypeArguments.Any(IsOrReferencesErrorType),
            _ => false,
        };
    }

    // Picked from https://github.com/YairHalberstadt/stronginject Thank you!
    public static bool IsAccessibleInternally(this ITypeSymbol type)
    {
        if (type is ITypeParameterSymbol)
            return true;
        if (!type.ContainingType?.IsAccessibleInternally() ?? false)
            return false;
        return type switch
        {
            IArrayTypeSymbol array => array.ElementType.IsAccessibleInternally(),
            IPointerTypeSymbol pointer => pointer.PointedAtType.IsAccessibleInternally(),
            INamedTypeSymbol named => named.DeclaredAccessibility is Accessibility.Public or Accessibility.ProtectedOrInternal or Accessibility.Internal
                                      && named.TypeArguments.All(IsAccessibleInternally),
            _ => false,
        };
    }

    // Picked from https://github.com/YairHalberstadt/stronginject Thank you!
    public static bool IsAccessiblePublicly(this ITypeSymbol type)
    {
        if (type is ITypeParameterSymbol)
            return true;
        if (!type.ContainingType?.IsAccessiblePublicly() ?? false)
            return false;
        return type switch
        {
            IArrayTypeSymbol array => array.ElementType.IsAccessiblePublicly(),
            IPointerTypeSymbol pointer => pointer.PointedAtType.IsAccessiblePublicly(),
            INamedTypeSymbol named => named.DeclaredAccessibility is Accessibility.Public
                                      && named.TypeArguments.All(IsAccessiblePublicly),
            _ => false,
        };
    }

    // Picked from https://github.com/YairHalberstadt/stronginject Thank you!
    public static string FullName(
        this ITypeSymbol type,
        SymbolDisplayMiscellaneousOptions miscellaneousOptions = SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier) =>
        type.ToDisplayString(new SymbolDisplayFormat(
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            parameterOptions: SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeParamsRefOut,
            memberOptions: SymbolDisplayMemberOptions.IncludeRef,
            miscellaneousOptions: miscellaneousOptions));
}