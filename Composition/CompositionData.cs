using System.Collections.Immutable;
using ContainerFeatureSampleComparison.FeatureDefinitions;

namespace ContainerFeatureSampleComparison.Composition;

internal record CompositionData(
    IReadOnlyList<FeatureGroupDescription> FeatureGroupDescriptions,
    IReadOnlyList<string> DiContainerNames,
    ImmutableDictionary<(string DiContainerName, Feature Feature), IFeatureDescription> AllAvailableFeatureDescriptions,
    ImmutableDictionary<(string DiContainerName, MiscellaneousInformation Kind), string> AllAvailableMiscellaneousInformation,
    ImmutableDictionary<object, string> IdMap,
    IImmutableDictionary<string, ResolutionStage?> AllResolutionStage)
{
    internal static CompositionData Create()
    {
        var diContainerComparison = Generated.Descriptions.CreateDiContainerComparison();
        var featureGroupDescriptions = Generated.Descriptions.CreateFeatureGroupDescriptions();

        var diContainerNames = diContainerComparison.DiContainerDescriptions.Keys.OrderBy(x => x).ToList();

        var allAvailableFeatureDescriptions = diContainerComparison
            .DiContainerDescriptions
            .SelectMany(kvpContainer =>
                kvpContainer.Value.FeatureSamples.Select(kvpSamples => (
                    DiContainerName: kvpContainer.Key, 
                    Feature: kvpSamples.Key, 
                    FeatureDescription: kvpSamples.Value)))
            .ToImmutableDictionary(t => (t.DiContainerName, t.Feature), t => t.FeatureDescription);

        var allAvailableMiscellaneousInformation = diContainerComparison
            .DiContainerDescriptions
            .SelectMany(kvpContainer =>
                kvpContainer.Value.MiscellaneousInformation.Select(kvpSamples => (
                    DiContainerName: kvpContainer.Key, 
                    Kind: kvpSamples.Key, 
                    Information: kvpSamples.Value)))
            .ToImmutableDictionary(t => (t.DiContainerName, t.Kind), t => t.Information);

        var idMap = allAvailableFeatureDescriptions
            .Values
            .OfType<object>()
            .Concat(featureGroupDescriptions.SelectMany(g => g.Features))
            .Select((fd, i) => (fd, $"id{i}"))
            .ToImmutableDictionary(t => t.fd, t => t.Item2);

        var allResolutionStage = diContainerComparison
            .DiContainerDescriptions
            .Select(kvpContainer => (DiContainerName: kvpContainer.Key, kvpContainer.Value.ResolutionStage))
            .ToImmutableDictionary(t => t.DiContainerName, t => t.ResolutionStage);

        return new CompositionData(
            featureGroupDescriptions,
            diContainerNames,
            allAvailableFeatureDescriptions,
            allAvailableMiscellaneousInformation,
            idMap,
            allResolutionStage);
    }
}