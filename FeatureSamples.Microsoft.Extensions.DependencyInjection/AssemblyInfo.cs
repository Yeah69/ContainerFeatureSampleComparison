using ContainerFeatureSampleComparison.FeatureDefinitions;

[assembly:DiContainer("Microsoft.Extensions.DependencyInjection")]

[assembly:MiscellaneousInformation(MiscellaneousInformation.RepositoryUrl, "https://github.com/dotnet/runtime")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.DocumentationUrl, "https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=dotnet-plat-ext-7.0")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.LicenseUrl, "https://licenses.nuget.org/MIT")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.NugetPackageUrl, "https://www.nuget.org/packages/MrMeeseeks.DIE/")]

[assembly:ResolutionStage(ResolutionStage.RunTime)]

[assembly:MissingFeature(Feature.FactoriesFunc, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support factories (Func<T>/Lazy<T>): https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#default-service-container-replacement")]
[assembly:MissingFeature(Feature.FactoriesFuncWithParameter, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support factories (Func<T>/Lazy<T>): https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#default-service-container-replacement")]
[assembly:MissingFeature(Feature.FactoriesFuncWithParameterForSubDependencies, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support factories (Func<T>/Lazy<T>): https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#default-service-container-replacement")]
[assembly:MissingFeature(Feature.FactoriesLazy, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support factories (Func<T>/Lazy<T>): https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#default-service-container-replacement")]
