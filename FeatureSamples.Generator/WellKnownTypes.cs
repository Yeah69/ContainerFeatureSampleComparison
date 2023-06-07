using System.Collections.Generic;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using ContainerFeatureSampleComparison.FeatureSamples.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace ContainerFeatureSampleComparison.FeatureSamples.Generator;

internal record WellKnownTypes(
    // Attributes
    INamedTypeSymbol DiContainerAttributeType,
    INamedTypeSymbol FeatureSampleAttributeType,
    INamedTypeSymbol MissingFeatureAttributeType,
    INamedTypeSymbol MiscellaneousInformationAttributeType,
    INamedTypeSymbol ResolutionStageAttributeType,
    
    // Enums
    INamedTypeSymbol FeatureType,
    INamedTypeSymbol MissingFeatureReasonType,
    INamedTypeSymbol ResolutionStageType,
    INamedTypeSymbol MiscellaneousInformationType,
    
    // Records
    INamedTypeSymbol FeatureSampleDescriptionType,
    INamedTypeSymbol MissingFeatureDescriptionType,
    INamedTypeSymbol DiContainerDescriptionType,

    // Other
    INamedTypeSymbol DictionaryOfFeatureAndIFeatureDescriptionType,
    INamedTypeSymbol DictionaryOfMiscellaneousInformationAndStringType)
{
    internal static WellKnownTypes Create(Compilation compilation)
    {
        var dictionaryType = compilation.GetTypeByMetadataNameOrThrow(typeof(Dictionary<,>).FullName);
        var featureType = compilation.GetTypeByMetadataNameOrThrow(typeof(Feature).FullName);
        var diContainerDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(DiContainerDescription).FullName);
        var iFeatureDescriptionType = compilation.GetTypeByMetadataNameOrThrow(typeof(IFeatureDescription).FullName);
        var miscellaneousInformationType = compilation.GetTypeByMetadataNameOrThrow(typeof(MiscellaneousInformation).FullName);
        var stringType = compilation.GetTypeByMetadataNameOrThrow(typeof(string).FullName);

        return new WellKnownTypes(
            // Attributes
            DiContainerAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(DiContainerAttribute).FullName),
            FeatureSampleAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureSampleAttribute).FullName),
            MissingFeatureAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(MissingFeatureAttribute).FullName),
            MiscellaneousInformationAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(MiscellaneousInformationAttribute).FullName),
            ResolutionStageAttributeType: compilation.GetTypeByMetadataNameOrThrow(typeof(ResolutionStageAttribute).FullName),
            
            // Enums
            FeatureType: featureType,
            MissingFeatureReasonType: compilation.GetTypeByMetadataNameOrThrow(typeof(MissingFeatureReason).FullName),
            ResolutionStageType: compilation.GetTypeByMetadataNameOrThrow(typeof(ResolutionStage).FullName),
            MiscellaneousInformationType: miscellaneousInformationType,
            
            // Records (and interfaces)
            FeatureSampleDescriptionType: compilation.GetTypeByMetadataNameOrThrow(typeof(FeatureSampleDescription).FullName),
            MissingFeatureDescriptionType: compilation.GetTypeByMetadataNameOrThrow(typeof(MissingFeatureDescription).FullName),
            DiContainerDescriptionType: diContainerDescriptionType,
            
            // Other
            DictionaryOfFeatureAndIFeatureDescriptionType: dictionaryType.Construct(featureType, iFeatureDescriptionType),
            DictionaryOfMiscellaneousInformationAndStringType: dictionaryType.Construct(miscellaneousInformationType, stringType));
    }
}