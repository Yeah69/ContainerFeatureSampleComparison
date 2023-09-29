using ContainerFeatureSampleComparison.FeatureDefinitions;

[assembly:DiContainer("MrMeeseeks.DIE")]

[assembly:MiscellaneousInformation(MiscellaneousInformation.RepositoryUrl, "https://github.com/Yeah69/MrMeeseeks.DIE")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.DocumentationUrl, "https://die.mrmeeseeks.dev")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.LicenseUrl, "https://github.com/Yeah69/MrMeeseeks.DIE/blob/main/LICENSE")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.NugetPackageUrl, "https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection")]

[assembly:ResolutionStage(ResolutionStage.CompileTime)]

[assembly:MissingFeature(Feature.KeyedInjectionsKeyValueInjection, MissingFeatureReason.DesignDecision, "Imagined use cases didn't justify the implementation effort. If this particular feature is still crucial for you, let's get into discussion about it: https://github.com/Yeah69/MrMeeseeks.DIE/discussions")]