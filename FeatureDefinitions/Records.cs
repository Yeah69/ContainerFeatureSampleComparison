namespace ContainerFeatureSampleComparison.FeatureDefinitions;

public interface IFeatureDescription
{
    Feature Feature { get; }
}

public record FeatureSampleDescription(Feature Feature, string SampleCode) : IFeatureDescription;

public record MissingFeatureDescription(Feature Feature, MissingFeatureReason Reason, string? Hint) : IFeatureDescription;

public record DiContainerDescription(
    string ContainerName, 
    ResolutionStage? ResolutionStage,
    IReadOnlyDictionary<Feature, IFeatureDescription> FeatureSamples,
    IReadOnlyDictionary<MiscellaneousInformation, string> MiscellaneousInformation);

public record DiContainerComparison(IReadOnlyDictionary<string, DiContainerDescription> DiContainerDescriptions);

public record FeatureDescription(Feature Feature, string Title, string Description);

public record FeatureGroupDescription(FeatureGroup FeatureGroup, string Title, string Description, IReadOnlyList<FeatureDescription> Features);
