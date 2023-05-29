using ContainerFeatureSampleComparison.FeatureDefinitions;

[assembly:DiContainer("MrMeeseeks.DIE")]

[assembly:MiscellaneousInformation(MiscellaneousInformation.RepositoryUrl, "https://github.com/Yeah69/MrMeeseeks.DIE")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.DocumentationUrl, "https://die.mrmeeseeks69.dev")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.LicenseUrl, "https://github.com/Yeah69/MrMeeseeks.DIE/blob/main/LICENSE")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.NuGetPackageUrl, "https://www.nuget.org/packages/MrMeeseeks.DIE/")]

[assembly:ResolutionStage(ResolutionStage.CompileTime)]

[assembly:MissingFeature(Feature.ConstructorMultipleThenMostParameters, MissingFeatureReason.DesignDecision, "When a type has multiple constructors, then the constructor to be used is supposed to be configured explicitly.")]
