using ContainerFeatureSampleComparison.FeatureDefinitions;

[assembly:DiContainer("MrMeeseeks.DIE")]

[assembly:MiscellaneousInformation(MiscellaneousInformation.RepositoryUrl, "https://github.com/Yeah69/MrMeeseeks.DIE")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.DocumentationUrl, "https://die.mrmeeseeks.dev")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.LicenseUrl, "https://github.com/Yeah69/MrMeeseeks.DIE/blob/main/LICENSE")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.NugetPackageUrl, "https://www.nuget.org/packages/MrMeeseeks.DIE/")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.NonUrlInformation, "An information that isn't an URL.")]

[assembly:ResolutionStage(ResolutionStage.CompileTime)]

[assembly:MissingFeature(Feature.ConstructorMultipleThenMostParameters, MissingFeatureReason.DesignDecision, "When a type has multiple constructors, then the constructor to be used is supposed to be configured explicitly.")]
[assembly:MissingFeature(Feature.NotSupported, MissingFeatureReason.NotSupported, "This feature is not supported, because of technical reasons.")]
[assembly:MissingFeature(Feature.Unimplemented, MissingFeatureReason.Unimplemented, "This feature is not implemented yet. But it is planned to be implemented in the future.")]
[assembly:MissingFeature(Feature.DesignDecision, MissingFeatureReason.DesignDecision, "This feature is not implemented because of a design decision in favor of ….")]
[assembly:MissingFeature(Feature.NotSupportedNoHint, MissingFeatureReason.NotSupported)]
[assembly:MissingFeature(Feature.UnimplementedNoHint, MissingFeatureReason.Unimplemented)]
[assembly:MissingFeature(Feature.DesignDecisionNoHint, MissingFeatureReason.DesignDecision)]
