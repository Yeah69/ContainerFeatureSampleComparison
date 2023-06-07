using System.Linq;
using System.Text;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.SampleAggregationGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace ContainerFeatureSampleComparison.SampleAggregationGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var wellKnownTypesProvider = context.CompilationProvider.Select((compilation, _) => WellKnownTypes.Create(compilation));
        var diComparisonCodeProvider = context.CompilationProvider.SelectMany((compilation, _) => compilation
                .SourceModule
                .ReferencedAssemblySymbols
                .Select(a => a.GetTypeByMetadataName($"{a.Name}.Description"))
                .OfType<INamedTypeSymbol>())
            .Collect()
            .Combine(wellKnownTypesProvider)
            .Select((leftRight, _) => 
            {
                var (descriptionTypes, wellKnownTypes) = leftRight;
                var code = new StringBuilder();
                code.AppendLine($$"""
public static {{wellKnownTypes.DiContainerComparisonType.FullName()}} CreateDiContainerComparison() 
{
var diContainerDescriptions = new {{wellKnownTypes.DictionaryOfStringAndDiContainerDescriptionType.FullName()}}();
""");
                
                for (var i = 0; i < descriptionTypes.Length; i++)
                {
                    code.AppendLine($$"""
var desc{{i}} = {{descriptionTypes[i].FullName()}}.Create();
diContainerDescriptions[desc{{i}}.ContainerName] = desc{{i}};
""");
                }
                
                code.AppendLine($$"""
return new {{wellKnownTypes.DiContainerComparisonType.FullName()}}(diContainerDescriptions);
}
""");
                return code.ToString();
            });
        
        var featureGroupDescriptionsCodeProvider = wellKnownTypesProvider.Select((wellKnownTypes, _) =>
        {
            var code = new StringBuilder();
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
        
            code.AppendLine("""
return ret;
}
""");
            return code.ToString();
        });
        
        
        context.RegisterSourceOutput(diComparisonCodeProvider.Combine(featureGroupDescriptionsCodeProvider),
            static (spc, leftRight) =>
            {
                var code = new StringBuilder();
                code.AppendLine($$"""
#nullable enable
namespace ContainerFeatureSampleComparison.Generated
{
public class Descriptions 
{
""");
                code.AppendLine(leftRight.Left);
                code.AppendLine(leftRight.Right);

                code.AppendLine("""
}
}
#nullable disable
""");
            
                var codeSource = CSharpSyntaxTree
                    .ParseText(SourceText.From(code.ToString(), Encoding.UTF8))
                    .GetRoot()
                    .NormalizeWhitespace()
                    .SyntaxTree
                    .GetText();
                    
                spc.AddSource("ContainerFeatureSampleComparison.Generated.Descriptions.g.cs", codeSource);
            });
    }
}