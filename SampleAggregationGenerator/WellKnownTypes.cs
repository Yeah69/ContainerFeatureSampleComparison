using System.Collections.Generic;
using System.Runtime.Serialization;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.SampleAggregationGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace ContainerFeatureSampleComparison.SampleAggregationGenerator;

internal record WellKnownTypes(
    // Attributes
    INamedTypeSymbol DiContainerAttributeType,
    INamedTypeSymbol FeatureSampleAttributeType,
    INamedTypeSymbol MissingFeatureAttributeType,
    INamedTypeSymbol MiscellaneousInformationAttributeType,
    INamedTypeSymbol ResolutionStageAttributeType,
    INamedTypeSymbol FeatureGroupAssignmentAttributeType,
    
    // Enums
    INamedTypeSymbol FeatureType,
    INamedTypeSymbol MissingFeatureReasonType,
    INamedTypeSymbol ResolutionStageType,
    INamedTypeSymbol MiscellaneousInformationType,
    INamedTypeSymbol FeatureGroupType,
    
    // Records
    INamedTypeSymbol FeatureSampleDescriptionType,
    INamedTypeSymbol MissingFeatureDescriptionType,
    INamedTypeSymbol DiContainerDescriptionType,
    INamedTypeSymbol DiContainerComparisonType,
    INamedTypeSymbol FeatureDescriptionType,
    INamedTypeSymbol FeatureGroupDescriptionType,

    // Other
    INamedTypeSymbol DictionaryOfFeatureAndIFeatureDescriptionType,
    INamedTypeSymbol DictionaryOfMiscellaneousInformationAndStringType,
    INamedTypeSymbol DictionaryOfStringAndDiContainerDescriptionType,
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
        var iFeatureDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(IFeatureDescription).FullName);
        var miscellaneousInformationType = compilation.GetTypeByMetadataNameOrThrow(typeof(MiscellaneousInformation).FullName);
        var stringType = compilation.GetTypeByMetadataNameOrThrow(typeof(string).FullName);
        var featureGroupDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureGroupDescription).FullName);
        var iReadOnlyListType = compilation.GetTypeByMetadataNameOrThrow(typeof(IReadOnlyList<>).FullName);
        var listType = compilation.GetTypeByMetadataNameOrThrow(typeof(List<>).FullName);
        var featureDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureDescription).FullName);

        return new WellKnownTypes(
            // Attributes
            DiContainerAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(DiContainerAttribute).FullName),
            FeatureSampleAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureSampleAttribute).FullName),
            MissingFeatureAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(MissingFeatureAttribute).FullName),
            MiscellaneousInformationAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(MiscellaneousInformationAttribute).FullName),
            ResolutionStageAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(ResolutionStageAttribute).FullName),
            FeatureGroupAssignmentAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureGroupAssignmentAttribute).FullName),
            
            // Enums
            FeatureType: featureType,
            MissingFeatureReasonType: compilation.GetTypeByMetadataNameOrThrow(typeof(MissingFeatureReason).FullName),
            ResolutionStageType: compilation.GetTypeByMetadataNameOrThrow(typeof(ResolutionStage).FullName),
            MiscellaneousInformationType: miscellaneousInformationType,
            FeatureGroupType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureGroup).FullName),
            
            // Records (and interfaces)
            FeatureSampleDescriptionType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureSampleDescription).FullName),
            MissingFeatureDescriptionType: compilation.GetTypeByMetadataNameOrThrow(typeof(MissingFeatureDescription).FullName),
            DiContainerDescriptionType: diContainerDescriptionType,
            DiContainerComparisonType: compilation.GetTypeByMetadataNameOrThrow(typeof(DiContainerComparison).FullName),
            FeatureDescriptionType: featureDescriptionType,
            FeatureGroupDescriptionType: featureGroupDescriptionType,
            
            // Other
            DictionaryOfFeatureAndIFeatureDescriptionType: dictionaryType.Construct(featureType, iFeatureDescriptionType),
            DictionaryOfMiscellaneousInformationAndStringType: dictionaryType.Construct(miscellaneousInformationType, stringType),
            DictionaryOfStringAndDiContainerDescriptionType: dictionaryType.Construct(stringType, diContainerDescriptionType),
            IReadOnlyListOfFeatureGroupDescriptionType: iReadOnlyListType.Construct(featureGroupDescriptionType),
            ListOfFeatureGroupDescriptionType: listType.Construct(featureGroupDescriptionType),
            ListOfFeatureDescriptionType: listType.Construct(featureDescriptionType),
            EnumMemberAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(EnumMemberAttribute).FullName));
    }
}