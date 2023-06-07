using System;
using Microsoft.CodeAnalysis;

namespace ContainerFeatureSampleComparison.FeatureSamples.Generator.Extensions;

public static class CompilationExtension
{
    public static INamedTypeSymbol GetTypeByMetadataNameOrThrow(this Compilation compilation, string metadataName) =>
        compilation.GetTypeByMetadataName(metadataName) 
        ?? throw new ArgumentException("Type not found by metadata name.", nameof(metadataName));
}