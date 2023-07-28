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

[assembly:MissingFeature(Feature.TypeInitializersAsyncTask, MissingFeatureReason.NotSupported, "One could implement it in analogous fashion as the Sync-sample. However because there is no support for async factories (which return either ValueTask<T>/Task<T>), the async initialization is unsafe, as it either forces a blocking call or a fire/forget-handling (which may lead to race conditions).")]
[assembly:MissingFeature(Feature.TypeInitializersAsyncValueTask, MissingFeatureReason.NotSupported, "One could implement it in analogous fashion as the Sync-sample. However because there is no support for async factories (which return either ValueTask<T>/Task<T>), the async initialization is unsafe, as it either forces a blocking call or a fire/forget-handling (which may lead to race conditions).")]

[assembly:MissingFeature(Feature.InjectionsExplicitPropertyChoice, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support property injection: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#default-service-container-replacement")]
[assembly:MissingFeature(Feature.InjectionsInitPropertyImplicit, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support property injection: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#default-service-container-replacement")]

[assembly:MissingFeature(Feature.AsyncNotWrapped, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support async initialization (see async samples in \"Type Initializers\" group).")]
[assembly:MissingFeature(Feature.AsyncWrappedTask, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support async initialization (see async samples in \"Type Initializers\" group).")]
[assembly:MissingFeature(Feature.AsyncWrappedValueTask, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support async initialization (see async samples in \"Type Initializers\" group).")]
[assembly:MissingFeature(Feature.AsyncNotWrappedIEnumerable, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support async initialization (see async samples in \"Type Initializers\" group).")]
[assembly:MissingFeature(Feature.AsyncWrappedIAsyncEnumerable, MissingFeatureReason.NotSupported, "Microsoft.Extensions.DependencyInjection does not support async initialization (see async samples in \"Type Initializers\" group).")]
