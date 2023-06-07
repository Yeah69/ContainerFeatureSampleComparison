using System.Collections.Immutable;
using System.Linq;
using System.Text;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.FeatureSamples.Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace ContainerFeatureSampleComparison.FeatureSamples.Generator;

internal record AssemblyData(
    string? ContainerName,
    string Namespace,
    WellKnownTypes WellKnownTypes);

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        const string featureSamplesIdentifier = "featureSamples";
        const string miscellaneousInformationIdentifier = "miscellaneousInformation";
        const string resolutionStageIdentifier = "resolutionStage";
        
        var wellKnownTypesProvider = context.CompilationProvider.Select((compilation, _) => WellKnownTypes.Create(compilation));
        var attributesProvider = context.CompilationProvider.Combine(wellKnownTypesProvider)
            .SelectMany((leftRight, _) =>
            {
                var wellKnownTypes = leftRight.Right;
                return leftRight.Left.Assembly.GetAttributes()
                    .Where(ad =>
                        SymbolEqualityComparer.Default.Equals(ad.AttributeClass, wellKnownTypes.FeatureSampleAttributeType)
                        || SymbolEqualityComparer.Default.Equals(ad.AttributeClass, wellKnownTypes.MissingFeatureAttributeType)
                        || SymbolEqualityComparer.Default.Equals(ad.AttributeClass, wellKnownTypes.MiscellaneousInformationAttributeType)
                        || SymbolEqualityComparer.Default.Equals(ad.AttributeClass, wellKnownTypes.ResolutionStageAttributeType));
            });

        var assemblyDataProvider = context.CompilationProvider.Combine(wellKnownTypesProvider).Select((leftRight, _) =>
            new AssemblyData(
                leftRight.Left
                    .Assembly
                    .GetAttributes()
                    .Single(ad =>
                        SymbolEqualityComparer.Default.Equals(leftRight.Right.DiContainerAttributeType,
                            ad.AttributeClass))
                    .ConstructorArguments[0]
                    .Value as string,
                leftRight.Left.Assembly.Name,
                leftRight.Right));

        var attributesCodeProvider = attributesProvider.Combine(wellKnownTypesProvider)
            .Select((leftRight, _) => GenerateAttributeCode(leftRight.Left, leftRight.Right))
            .Collect();
        
        var codeProvider = assemblyDataProvider.Combine(attributesCodeProvider).Select((leftRight, _) => 
            GenerateCode(leftRight.Left, leftRight.Right));
        
        context.RegisterSourceOutput(codeProvider,
            static (spc, code) =>
            {
                var codeSource = CSharpSyntaxTree
                    .ParseText(SourceText.From(code.ToString(), Encoding.UTF8))
                    .GetRoot()
                    .NormalizeWhitespace()
                    .SyntaxTree
                    .GetText();
                    
                spc.AddSource("Description.g.cs", codeSource);
            });

        string? GenerateAttributeCode(AttributeData attribute, WellKnownTypes wellKnownTypes)
        {
            if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass,
                    wellKnownTypes.FeatureSampleAttributeType)
                || SymbolEqualityComparer.Default.Equals(attribute.AttributeClass,
                    wellKnownTypes.MissingFeatureAttributeType))
            {
                var feature = (Feature?)(int?)attribute.ConstructorArguments[0].Value ??
                              Feature.RegisterResolveInterface;
                var sampleText = attribute.ApplicationSyntaxReference is { } a
                    ? a.GetSyntax().GetLocation().SourceTree is { } b
                        ? b.GetText().ToString()
                        : "InnerNull"
                    : "OuterNull";
                if (SymbolEqualityComparer.Default.Equals(wellKnownTypes.FeatureSampleAttributeType,
                        attribute.AttributeClass))
                    return
                        $"featureSamples[{wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}] = new {wellKnownTypes.FeatureSampleDescriptionType.FullName()}({wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}, {
                            SymbolDisplay.FormatLiteral(sampleText, true)});";

                var missingFeatureReason = (MissingFeatureReason?)(int?)attribute.ConstructorArguments[1].Value ??
                                           MissingFeatureReason.Unimplemented;
                var hint = attribute.ConstructorArguments.Length > 2
                    ? (string?)attribute.ConstructorArguments[2].Value
                    : null;
                return
                    $"featureSamples[{wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}] = new {wellKnownTypes.MissingFeatureDescriptionType.FullName()}({wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}, {wellKnownTypes.MissingFeatureReasonType.FullName()}.{missingFeatureReason.ToString()}, {
                        (hint is null ? "null" : SymbolDisplay.FormatLiteral(hint, true))});";
            }

            if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass,
                    wellKnownTypes.MiscellaneousInformationAttributeType))
            {
                var miscellaneousInformation =
                    (MiscellaneousInformation?)(int?)attribute.ConstructorArguments[0].Value ??
                    MiscellaneousInformation.RepositoryUrl;
                return
                    $"miscellaneousInformation[{wellKnownTypes.MiscellaneousInformationType.FullName()}.{miscellaneousInformation.ToString()}] = {
                        SymbolDisplay.FormatLiteral(attribute.ConstructorArguments[1].Value as string ?? "", true)};";
            }

            if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass,
                    wellKnownTypes.ResolutionStageAttributeType))
            {
                var resolutionStage =
                    ((ResolutionStage?)(int?)attribute.ConstructorArguments[0].Value)?.ToString() 
                    ?? "null";
                return $"resolutionStage = {wellKnownTypes.ResolutionStageType.FullName()}.{resolutionStage};";
            }

            return null;
        }

        string GenerateCode(AssemblyData assemblyData, ImmutableArray<string?> attributeLines)
        {
            var wellKnownTypes = assemblyData.WellKnownTypes;
            var code = new StringBuilder();
            code.AppendLine($$"""
#nullable enable
namespace {{assemblyData.Namespace}}
{
public static class Description
{
public static {{wellKnownTypes.DiContainerDescriptionType.FullName()}} Create() 
{
var {{featureSamplesIdentifier}} = new {{wellKnownTypes.DictionaryOfFeatureAndIFeatureDescriptionType.FullName()}}();
var {{miscellaneousInformationIdentifier}} = new {{wellKnownTypes.DictionaryOfMiscellaneousInformationAndStringType.FullName()}}();
{{wellKnownTypes.ResolutionStageType.FullName()}}? {{resolutionStageIdentifier}} = null;
""");
            
            foreach (var attributeLine in attributeLines.OfType<string>())
                code.AppendLine(attributeLine);

            code.AppendLine($$"""
return new {{wellKnownTypes.DiContainerDescriptionType.FullName()}}({{SymbolDisplay.FormatLiteral(assemblyData.ContainerName ?? "", true)}}, resolutionStage, featureSamples, miscellaneousInformation);
}
}
}
#nullable disable
""");
            return code.ToString();
        }
    }
}