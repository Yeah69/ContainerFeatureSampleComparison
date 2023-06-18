using System.Collections.Generic;
using System.Runtime.Serialization;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.SampleAggregationGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace ContainerFeatureSampleComparison.SampleAggregationGenerator;

internal record WellKnownTypes(
    // Attributes
    INamedTypeSymbol FeatureEnumInfoAttributeType,
    
    // Enums
    INamedTypeSymbol FeatureType,
    INamedTypeSymbol FeatureGroupType,
    
    // Records
    INamedTypeSymbol DiContainerComparisonType,
    INamedTypeSymbol FeatureDescriptionType,
    INamedTypeSymbol FeatureGroupDescriptionType,

    // Other
    INamedTypeSymbol DictionaryOfStringAndDiContainerDescriptionType,
    // ReSharper disable once InconsistentNaming
    INamedTypeSymbol IReadOnlyListOfFeatureGroupDescriptionType,
    INamedTypeSymbol ListOfFeatureGroupDescriptionType,
    INamedTypeSymbol ListOfFeatureDescriptionType,
    INamedTypeSymbol EnumMemberAttributeType)
{
    internal static WellKnownTypes Create(Compilation compilation)
    {
        var dictionaryType = compilation.GetTypeByMetadataNameOrThrow(typeof(Dictionary<,>).FullName);
        var featureType = compilation.GetTypeByMetadataNameOrThrow(typeof(Feature).FullName);
        var diContainerDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(DiContainerDescription).FullName);
        var stringType = compilation.GetTypeByMetadataNameOrThrow(typeof(string).FullName);
        var featureGroupDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureGroupDescription).FullName);
        var iReadOnlyListType = compilation.GetTypeByMetadataNameOrThrow(typeof(IReadOnlyList<>).FullName);
        var listType = compilation.GetTypeByMetadataNameOrThrow(typeof(List<>).FullName);
        var featureDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureDescription).FullName);

        return new WellKnownTypes(
            // Attributes
            FeatureEnumInfoAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureEnumInfoAttribute).FullName),
            
            // Enums
            FeatureType: featureType,
            FeatureGroupType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureGroup).FullName),
            
            // Records (and interfaces)
            DiContainerComparisonType: compilation.GetTypeByMetadataNameOrThrow(typeof(DiContainerComparison).FullName),
            FeatureDescriptionType: featureDescriptionType,
            FeatureGroupDescriptionType: featureGroupDescriptionType,
            
            // Other
            DictionaryOfStringAndDiContainerDescriptionType: dictionaryType.Construct(stringType, diContainerDescriptionType),
            IReadOnlyListOfFeatureGroupDescriptionType: iReadOnlyListType.Construct(featureGroupDescriptionType),
            ListOfFeatureGroupDescriptionType: listType.Construct(featureGroupDescriptionType),
            ListOfFeatureDescriptionType: listType.Construct(featureDescriptionType),
            EnumMemberAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(EnumMemberAttribute).FullName));
    }
}