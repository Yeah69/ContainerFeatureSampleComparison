using System.Runtime.Serialization;

namespace ContainerFeatureSampleComparison.FeatureDefinitions;

public enum Feature
{
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.BasicTypes, 
        Title = "Concrete Class",
        Description = "A specified concrete class should be registrable and resolvable.")]
    RegisterResolveConcreteClass,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.BasicTypes, 
        Title = "Struct",
        Description = "A specified struct should be registrable and resolvable.")]
    RegisterResolveStruct,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.BasicTypes, 
        Title = "Record",
        Description = "A specified record should be registrable and resolvable.")]
    RegisterResolveRecord,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.BasicTypes, 
        Title = "Struct Record",
        Description = "A specified value record should be registrable and resolvable.")]
    RegisterResolveStructRecord,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.BasicTypes, 
        Title = "Interface",
        Description = "An implementing type (concrete class, struct, record) should be assignable to a specified interface, so that it is resolvable.")]
    RegisterResolveInterface,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.BasicTypes, 
        Title = "Abstract Class",
        Description = "An implementing type (concrete class, struct, record) should be assignable to a specified interface, so that it is resolvable.")]
    RegisterResolveAbstractClass,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Constructor, 
        Title = "Take Most Parameters When Multiple Constructors",
        Description = "If a instantiated type has multiple constructors, the container chooses the constructor with the most resolvable parameters.")]
    ConstructorMultipleThenMostParameters,
    
    ConstructorInjectionVanilla,
    ConstructorInjectionNullableNullCase,
    ConstructorInjectionNullableNotNullCase,
    ConstructorInjectionExplicitConstructorChoice,
    ConstructorInjectionStructParameterlessConstructorIgnored,
    ConstructorInjectionRecordCopyConstructorIgnored,
    
    PropertyInjectionImplicitForInit,
    PropertyInjectionExplicitChoice,
    PropertyInjectionNullableNullCase,
    PropertyInjectionNullableNotNullCase,
    
    CollectionInjectionIEnumerable,
    CollectionInjectionArray,
    CollectionInjectionIReadOnlyList,
    CollectionInjectionIList,
    CollectionInjectionReadOnlyList,
    CollectionInjectionList,
    CollectionInjectionRemainingCollectionTypes,
    CollectionInjectionCollectionChoice,
    
    TupleInjectionTuple,
    TupleInjectionValueTupleSyntax,
    TupleInjectionValueTuple,
    
    ScopeVanilla,
    ScopeTransientScope,
    
    ScopedInstanceScope,
    ScopedInstanceContainer,
    
    GenericsVanilla,
    GenericsOpenFillIns,
    
    TypeInitializerVanilla,
    TypeInitializerWithParameters,
    TypeInitializerNullableNullCase,
    TypeInitializerNullableNotNullCase,
    
    AsyncSupportAsyncTypeInitializer,
    AsyncSupportNotWrapped,
    AsyncSupportWrappedTask,
    AsyncSupportWrappedValueTask,
    AsyncSupportWrappedIAsyncEnumerable,
    
    DisposalIDisposable,
    DisposalIAsyncDisposable,
    DisposalTransient,
    DisposalAsyncTransient,
    DisposalScope,
    
    CustomizationCustomFactory,
    CustomizationCustomParameterInjection,
    CustomPropertyInjection,
    
    DecoratorPatternSingleDecorator,
    DecoratorPatternMultipleDecorators,
    DecoratorPatternExplicitOrder,
    DecoratorPatternDifferentOrderPerType,
    DecoratorPatternDefaultOrder,
    
    CompositePatternIEnumerable,
    CompositePatternDecorated,
    
    MiscInitializedInstanceContainer,
    MiscInitializedInstanceScope,
    MiscMarkerInterface,
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
    ConstructorInjection,
    PropertyInjection,
    CollectionInjection,
    FactoryInjection,
    TupleInjection,
    Scope,
    ScopedInstances,
    Generics,
    TypeInitializers,
    AsyncSupport,
    Disposal,
    Customization,
    DecoratorPattern,
    CompositePattern,
    Misc,
    [EnumMember(Value = "Group of features that don't have an specific feature group yet.")]
    Other,
}