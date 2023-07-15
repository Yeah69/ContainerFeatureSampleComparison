using ContainerFeatureSampleComparison.FeatureDefinitions;

[assembly:DiContainer("Microsoft.Extensions.DependencyInjection")]

[assembly:MiscellaneousInformation(MiscellaneousInformation.RepositoryUrl, "https://github.com/dotnet/runtime")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.DocumentationUrl, "https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=dotnet-plat-ext-7.0")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.LicenseUrl, "https://licenses.nuget.org/MIT")]
[assembly:MiscellaneousInformation(MiscellaneousInformation.NugetPackageUrl, "https://www.nuget.org/packages/MrMeeseeks.DIE/")]

[assembly:ResolutionStage(ResolutionStage.RunTime)]

[assembly:MissingFeature(Feature.ImplementationsStruct, MissingFeatureReason.NotSupported, "Structs are not supported by Microsoft.Extensions.DependencyInjection (e.g. `AddTransient<T>()` only accepts reference types as generic parameter).")]
[assembly:MissingFeature(Feature.ImplementationsStructRecord, MissingFeatureReason.NotSupported, "Structs are not supported by Microsoft.Extensions.DependencyInjection (e.g. `AddTransient<T>()` only accepts reference types as generic parameter).")]
[assembly:MissingFeature(Feature.ImplementationsStructParameterlessConstructorIgnored, MissingFeatureReason.NotSupported, "Structs are not supported by Microsoft.Extensions.DependencyInjection (e.g. `AddTransient<T>()` only accepts reference types as generic parameter).")]
[assembly:MissingFeature(Feature.ImplementationsNullableStructNotNullCase, MissingFeatureReason.NotSupported, "Structs are not supported by Microsoft.Extensions.DependencyInjection (e.g. `AddTransient<T>()` only accepts reference types as generic parameter).")]
[assembly:MissingFeature(Feature.ImplementationsNullableStructNullCase, MissingFeatureReason.NotSupported, "Structs are not supported by Microsoft.Extensions.DependencyInjection (e.g. `AddTransient<T>()` only accepts reference types as generic parameter).")]
