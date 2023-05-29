using System.Linq;
using System.Text;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.SampleAggregationGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace ContainerFeatureSampleComparison.SampleAggregationGenerator;

//[Generator]
public class Generator// : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // nothing to do
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var wellKnownTypes = WellKnownTypes.Create(context.Compilation);

        var sampleAssemblies = context
            .Compilation
            .SourceModule
            .ReferencedAssemblySymbols
            .Select(a => (Assembly: a, DIContainerAttributes: a.GetAttributes().Where(ad => SymbolEqualityComparer.Default.Equals(wellKnownTypes.DiContainerAttributeType, ad.AttributeClass)).ToList()))
            .Where(t => t.DIContainerAttributes.Count == 1)
            .Select(t => (t.Assembly, DIContainerAttribute: t.DIContainerAttributes.First()))
            .ToList();
        
        // todo: error handling for multiple assemblies with same container name

        var code = new StringBuilder();
        code.AppendLine($$"""
#nullable enable
namespace ContainerFeatureSampleComparison.Generated
{
public class Descriptions 
{
public static {{wellKnownTypes.DiContainerComparisonType.FullName()}} CreateDiContainerComparison() 
{
var diContainerDescriptions = new {{wellKnownTypes.DictionaryOfStringAndDiContainerDescriptionType.FullName()}}();
""");

        foreach (var (assembly, diContainerAttribute) in sampleAssemblies)
        {
            var containerName = diContainerAttribute
                .ConstructorArguments[0].Value as string;
            
            code.AppendLine($"var featureSamples = new {wellKnownTypes.DictionaryOfFeatureAndIFeatureDescriptionType.FullName()}();");

            var featureSampleAttributes = assembly
                .GetAttributes()
                .Where(ad => SymbolEqualityComparer.Default.Equals(wellKnownTypes.FeatureSampleAttributeType, ad.AttributeClass)
                    || SymbolEqualityComparer.Default.Equals(wellKnownTypes.MissingFeatureAttributeType, ad.AttributeClass))
                .GroupBy(ad => (Feature?)(int?)ad.ConstructorArguments[0].Value ?? Feature.RegisterResolveInterface);
            
            foreach (var group in featureSampleAttributes)
            {
                var feature = group.Key;
                if (group.SingleOrDefault() is { } attributeData)
                {
                    
                    var sampleText = attributeData.ApplicationSyntaxReference is {} a 
                        ? a.GetSyntax().GetLocation().SourceTree is {} b 
                            ? b.GetText().ToString()
                            : "InnerNull"
                        : "OuterNull";
                    if (SymbolEqualityComparer.Default.Equals(wellKnownTypes.FeatureSampleAttributeType, attributeData.AttributeClass))
                        code.AppendLine($"featureSamples[{wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}] = new {wellKnownTypes.FeatureSampleDescriptionType.FullName()}({wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}, {
                            SymbolDisplay.FormatLiteral(sampleText, true)});");
                    else
                    {
                        var missingFeatureReason = (MissingFeatureReason?)(int?)attributeData.ConstructorArguments[1].Value ?? MissingFeatureReason.Unimplemented;
                        var hint = attributeData.ConstructorArguments.Length > 2 ? (string?)attributeData.ConstructorArguments[2].Value : null;
                        code.AppendLine($"featureSamples[{wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}] = new {wellKnownTypes.MissingFeatureDescriptionType.FullName()}({wellKnownTypes.FeatureType.FullName()}.{feature.ToString()}, {wellKnownTypes.MissingFeatureReasonType.FullName()}.{missingFeatureReason.ToString()}, {
                            (hint is null ? "null" : SymbolDisplay.FormatLiteral(hint, true))});");
                    }
                }
                else
                {
                    // todo: error handling for multiple attributes on same feature
                }
            }
            
            var miscellaneousInformationAttributes = assembly
                .GetAttributes()
                .Where(ad => SymbolEqualityComparer.Default.Equals(wellKnownTypes.MiscellaneousInformationAttributeType, ad.AttributeClass))
                .GroupBy(ad => (MiscellaneousInformation?)(int?)ad.ConstructorArguments[0].Value ?? MiscellaneousInformation.RepositoryUrl);
            
            code.AppendLine($"var miscellaneousInformation = new {wellKnownTypes.DictionaryOfMiscellaneousInformationAndStringType.FullName()}();");

            foreach (var group in miscellaneousInformationAttributes)
            {
                var miscellaneousInformation = group.Key;
                if (group.SingleOrDefault() is { } attributeData)
                {
                    code.AppendLine($"miscellaneousInformation[{wellKnownTypes.MiscellaneousInformationType.FullName()}.{miscellaneousInformation.ToString()}] = {
                        SymbolDisplay.FormatLiteral(attributeData.ConstructorArguments[1].Value as string ?? "", true)};");
                }
                else
                {
                    // todo: error handling for multiple attributes on same miscellaneous information
                }
            }

            if (assembly
                .GetAttributes()
                .SingleOrDefault(ad =>
                    SymbolEqualityComparer.Default.Equals(wellKnownTypes.ResolutionStageAttributeType,
                        ad.AttributeClass)) is {} resolutionStageAttributeData)
            {
                var resolutionStage =
                    ((ResolutionStage?)(int?)resolutionStageAttributeData.ConstructorArguments[0].Value)?.ToString() 
                    ?? "null";
                code.AppendLine($"{wellKnownTypes.ResolutionStageType.FullName()}? resolutionStage = {wellKnownTypes.ResolutionStageType.FullName()}.{resolutionStage};");
            }
            else
            {
                code.AppendLine($"{wellKnownTypes.ResolutionStageType.FullName()}? resolutionStage = null;");
            }

            code.AppendLine($"diContainerDescriptions[{SymbolDisplay.FormatLiteral(containerName ?? "", true)}] = new {wellKnownTypes.DiContainerDescriptionType.FullName()}({SymbolDisplay.FormatLiteral(containerName ?? "", true)}, resolutionStage, featureSamples, miscellaneousInformation);");
        }

        code.AppendLine($$"""
return new {{wellKnownTypes.DiContainerComparisonType.FullName()}}(diContainerDescriptions);
}
""");
        //*
        code.AppendLine($$"""
public static {{wellKnownTypes.IReadOnlyListOfFeatureGroupDescriptionType.FullName()}} CreateFeatureGroupDescriptions() 
{
var ret = new {{wellKnownTypes.ListOfFeatureGroupDescriptionType.FullName()}}();
""");

        var groupedFeatureValues = wellKnownTypes
            .FeatureType
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Select(fs =>
            {
                var featureGroupAssignment = fs.GetAttributes().SingleOrDefault(ad => SymbolEqualityComparer.Default.Equals(wellKnownTypes.FeatureGroupAssignmentAttributeType, ad.AttributeClass));
                var enumMember = fs.GetAttributes().SingleOrDefault(ad => SymbolEqualityComparer.Default.Equals(wellKnownTypes.EnumMemberAttributeType, ad.AttributeClass));
                var featureGroupAssignmentValue = featureGroupAssignment is not null
                    ? ((FeatureGroup?)(int?)featureGroupAssignment.ConstructorArguments[0].Value) ?? FeatureGroup.Other
                    : FeatureGroup.Other;
                var enumMemberValue = enumMember is not null && enumMember.NamedArguments.SingleOrDefault(kvp => kvp.Key == "Value") is var valueKvp
                    ? valueKvp.Value.Value as string
                    : null;
                return (Feature: fs, FeatureGroupAssignment: featureGroupAssignmentValue, Description: enumMemberValue);
            })
            .GroupBy(t => t.FeatureGroupAssignment)
            .ToList();
        
        var featureGroupValues = wellKnownTypes
            .FeatureGroupType
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Select(fs =>
            {
                var enumMember = fs.GetAttributes().SingleOrDefault(ad => SymbolEqualityComparer.Default.Equals(wellKnownTypes.EnumMemberAttributeType, ad.AttributeClass));
                var enumMemberValue = enumMember is not null && enumMember.NamedArguments.SingleOrDefault(kvp => kvp.Key == "Value") is var valueKvp
                    ? valueKvp.Value.Value as string ?? ""
                    : "";
                return (FeatureGroupName: fs.Name, Description: enumMemberValue);
            })
            .ToDictionary(t => t.FeatureGroupName, t => t.Description);

        var i = 0;

        foreach (var groupedFeatureValue in groupedFeatureValues)
        {
            code.AppendLine($"var group{i} = new {wellKnownTypes.ListOfFeatureDescriptionType.FullName()}();");
            var featureGroup = groupedFeatureValue.Key;
            foreach (var (feature, _, enumMember) in groupedFeatureValue)
            {
                code.AppendLine($"group{i}.Add(new {wellKnownTypes.FeatureDescriptionType.FullName()}({wellKnownTypes.FeatureType.FullName()}.{feature.Name}, \"{feature.Name}\", \"{enumMember ?? ""}\"));");
            }
            code.AppendLine($"ret.Add(new {wellKnownTypes.FeatureGroupDescriptionType.FullName()}({wellKnownTypes.FeatureGroupType.FullName()}.{featureGroup.ToString()}, \"{featureGroup.ToString()}\", \"{featureGroupValues[featureGroup.ToString()]}\", group{i}));");
            i++;
        }
        
        code.AppendLine($$"""
return ret;
}
""");//*/
        
        
        code.AppendLine($$"""
}
}
#nullable disable
""");
        
        var partialCodeSource = CSharpSyntaxTree
            .ParseText(SourceText.From(code.ToString(), Encoding.UTF8))
            .GetRoot()
            .NormalizeWhitespace()
            .SyntaxTree
            .GetText();
                    
        context.AddSource("ContainerFeatureSampleComparison.Generated.Descriptions.g.cs", partialCodeSource);
    }
}