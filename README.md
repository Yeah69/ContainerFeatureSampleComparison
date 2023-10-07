# ContainerFeatureSampleComparison

Hi there! This projects goal is to gather a feature comparison of dependency injection (DI) container landscape in .Net.

The comparison can be accessed as a GitHub page: [https://yeah69.github.io/ContainerFeatureSampleComparison/](https://yeah69.github.io/ContainerFeatureSampleComparison/).

The contents of the page are automatically generated based on the contents of this repository. The idea is that the samples of the supported features are implemented in isolation and compilable.

This project is intended to be community driven. If you find a mistake or want to add a new container, feature or sample, then please feel free to open a pull request or  if you don't feel confident enough at least open a discussion or issue.

The next sections will explain how to add a new sample, feature or container to the comparison. We'll start with the smallest unit, the sample, and work our way up to the container.

## How to add a new sample

Requirements:
- A project for the associated container already exists in the repository (See "How to add a new container").
- A enum value for the associated feature already exists in the repository (See "How to add a new feature").

0. Create a new C#-file in the container project.
   - Recommendations: 
      - Choose or create a folder named by the feature group.
      - Name the file by the feature.
      - Put the feature/file name at the end of the namespace. That makes the isolation among the samples easier.
0. Add a `FeatureSampleAttribute` on assembly level (i.e. outside/before the namespace) and pass the feature enum value as parameter.
   - Each feature enum value can only be used once per container project.
   - However, a sample file can contain multiple attributes, if the sample implements multiple features.
0. Implement the sample.
   - Recommended structure:
      - Define types that are needed for the sample.
      - Define the container configuration.
      - Implement an example usage of the feature.
   - Recommendations:
      - Make the sample self-contained (isolated) if possible. In the comparison page only the contents of this file will be shown as the sample of the feature. So readers might not understand the sample if it is based on external code.
      - Add comments wherever it makes sense to highlight the important parts of the sample or explain something that might be not obvious.

## How to add a new feature

Requirements:
- A project for the associated container already exists in the repository (See "How to add a new container").

0. Optional: Add a new enum value to the `FeatureGroup` enum.
   - Skip this step if a fitting feature group already exists.
   - Please keep the enum value as short and concise as possible. The feature group title will be derived from the enum value.
   - The order matters! The feature groups will be listed sorted by the enum order.
   - Recommendation: Add a `EnumMember` to annotate a description to the feature group.
0. Add a new enum value to the `Feature` enum.
   - Please keep the enum value as short and concise as possible (if no `FeatureEnumInfoAttribute` used, then the title will be derived from the enum value).
   - The order matters! The features will be listed sorted by the feature group, first, and the enum order, second.
   - Recommendation: Add a `FeatureEnumInfoAttribute` to annotate a feature group, a title and a description to the feature.
0. Implement samples for the new feature for the containers that you feel confident with.
   - See "How to add a new sample" for details.

## How to add a new container

0. Create a new project.
   - Recommendation: Name the project by the container and prepend "FeatureSamples".
0. Add a `DiContainerAttribute` on assembly level (i.e. outside/before the namespace) and pass the container's name as parameter.
   - Recommendation: Put it into a file named `AssemblyInfo.cs` in the project's root folder.
0. Optional: Add a `ResolutionStageAttribute` & `MiscellaneousInformationAttribute`s on assembly level (i.e. outside/before the namespace) in order to provide non-feature information about the container.
   - Recommendation: Put it into a file named `AssemblyInfo.cs` in the project's root folder.
0. Implement samples for the features that you feel confident with.
    - See "How to add a new sample" for details.

## If a feature isn't supported by a container or just unknown

If the feature enum value isn't used, then it is automatically marked as unknown. So in this case nothing special has to be done.

However, a feature can be explicitly marked as missing with the `MissingFeatureAttribute` on assembly level (Recommendation: in the `AssemblyInfo.cs` file).

Three kinds of missing features can be distinguished:
- Unimplemented. The feature isn't excluded by design, but just not implemented yet.
- Unsupported. The feature couldn't be implemented for various reasons.
- Design decision. The feature is excluded by design.

The reasons for the feature being missing aside, the `MissingFeatureAttribute` can be used to annotate a description further specifying the situation or leading to helpful links.
