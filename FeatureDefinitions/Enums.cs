using System.Runtime.Serialization;

namespace ContainerFeatureSampleComparison.FeatureDefinitions;

public enum Feature
{
    [FeatureGroupAssignment(FeatureGroup.BasicTypes)]
    [EnumMember(Value = "A specified concrete class should be registrable and resolvable.")]
    RegisterResolveConcreteClass,
    [FeatureGroupAssignment(FeatureGroup.BasicTypes)]
    [EnumMember(Value = "A specified struct should be registrable and resolvable.")]
    RegisterResolveStruct,
    [FeatureGroupAssignment(FeatureGroup.BasicTypes)]
    [EnumMember(Value = "A specified record should be registrable and resolvable.")]
    RegisterResolveRecord,
    [FeatureGroupAssignment(FeatureGroup.BasicTypes)]
    [EnumMember(Value = "A specified value record should be registrable and resolvable.")]
    RegisterResolveStructRecord,
    [FeatureGroupAssignment(FeatureGroup.BasicTypes)]
    [EnumMember(Value = "An implementing type (concrete class, struct, record) should be assignable to a specified interface, so that it is resolvable.")]
    RegisterResolveInterface,
    [FeatureGroupAssignment(FeatureGroup.BasicTypes)]
    [EnumMember(Value = "An implementing type (concrete class, struct, record) should be assignable to a specified abstract class, so that it is resolvable.")]
    RegisterResolveAbstractClass,
    
    [FeatureGroupAssignment(FeatureGroup.Constructor)]
    [EnumMember(Value = "If a instantiated type has multiple constructors, the container chooses the constructor with the most resolvable parameters.")]
    ConstructorMultipleThenMostParameters,
    
    [EnumMember(Value = "This feature is not supported, because of technical reasons.")]
    NotSupported,
    [EnumMember(Value = "This feature is not implemented yet. But it is planned to be implemented in the future.")]
    Unimplemented,
    [EnumMember(Value = "This feature is not implemented because of a design decision in favor of ….")]
    DesignDecision,
    [EnumMember(Value = "This feature is not supported, because of technical reasons.")]
    NotSupportedNoHint,
    [EnumMember(Value = "This feature is not implemented yet. But it is planned to be implemented in the future.")]
    UnimplementedNoHint,
    [EnumMember(Value = "This feature is not implemented because of a design decision in favor of ….")]
    DesignDecisionNoHint,
    [EnumMember(Value = "The state of this feature is unknown.")]
    Unknown,
}

public enum MissingFeatureReason
{
    Unimplemented,
    NotSupported,
    DesignDecision,
}

public enum MiscellaneousInformation
{
    RepositoryUrl,
    LicenseUrl,
    NugetPackageUrl,
    DocumentationUrl,
    NonUrlInformation,
    Unknown,
}

public enum ResolutionStage
{
    RunTime,
    CompileTime,
}

public enum FeatureGroup
{
    [EnumMember(Value = "Group of features concerned with registration and resolution of basic types (implementations and abstractions).")]
    BasicTypes,
    [EnumMember(Value = "Group of features concerned with constructors.")]
    Constructor,
    [EnumMember(Value = "Group of features that don't have an specific feature group yet.")]
    Other,
}