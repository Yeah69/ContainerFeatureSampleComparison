using System.Runtime.Serialization;

namespace ContainerFeatureSampleComparison.FeatureDefinitions;

public enum Feature
{
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Concrete Class",
        Description = "A concrete (non-abstract) class is resolvable.")]
    ImplementationsConcreteClass,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Struct",
        Description = "A struct is resolvable.")]
    ImplementationsStruct,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Record",
        Description = "A record is resolvable.")]
    ImplementationsRecord,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Struct Record",
        Description = "A value record is resolvable.")]
    ImplementationsStructRecord,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Explicit Constructor Choice",
        Description = "If an implementation type has multiple constructors, the container is configurable to select a specific one.")]
    ImplementationsExplicitConstructorChoice,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Struct Parameterless Constructor Ignored",
        Description = "If a struct type has a non-parameterless constructor, then the parameterless constructor will be ignored.")]
    ImplementationsStructParameterlessConstructorIgnored,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Nullable Concrete Class (Null Case)",
        Description = "Injection of an instance of a nullable concrete class type. If the container doesn't know of the concrete class, it resolves to null.")]
    ImplementationsNullableConcreteClassNullCase,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Nullable Concrete Class (Not Null Case)",
        Description = "Injection of an instance of a nullable concrete class type. If the container knows the concrete class, it resolves to an instance of this type.")]
    ImplementationsNullableConcreteClassNotNullCase,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Nullable Struct (Null Case)",
        Description = "Injection of an instance of a nullable struct type. If the container doesn't know of the concrete class, it resolves to null.")]
    ImplementationsNullableStructNullCase,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Implementations, 
        Title = "Nullable Struct (Not Null Case)",
        Description = "Injection of an instance of a nullable struct type. If the container knows the concrete class, it resolves to an instance of this type.")]
    ImplementationsNullableStructNotNullCase,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Interface (Single Implementation)",
        Description = "Resolution of an interface type which has a single implementation type.")]
    AbstractionsInterfaceSingleImplementation,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Abstract Class (Single Implementation)",
        Description = "Resolution of an abstract class type which has a single implementation type.")]
    AbstractionsAbstractClassSingleImplementation,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Interface (Multiple Implementation)",
        Description = "Resolution of an interface type which has multiple implementation types.")]
    AbstractionsInterfaceMultipleImplementation,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Abstract Class (Multiple Implementation)",
        Description = "Resolution of an abstract class type which has multiple implementation types.")]
    AbstractionsAbstractClassMultipleImplementation,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Nullable Interface (Null Case)",
        Description = "Injection of an instance of a nullable interface type. If the container doesn't know of an implementation for this interface, it resolves to null.")]
    AbstractionsNullableInterfaceNullCase,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Nullable Interface (Not Null Case)",
        Description = "Injection of an instance of a nullable concrete class type. If the container knows of an implementation for this interface, it resolves to an instance of the implementation.")]
    AbstractionsNullableInterfaceNotNullCase,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Nullable Abstract Class (Null Case)",
        Description = "Injection of an instance of a nullable abstract class type. If the container doesn't know of an implementation for this abstract class type, it resolves to null.")]
    AbstractionsNullableAbstractClassNullCase,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Abstractions, 
        Title = "Nullable Abstract Class (Not Null Case)",
        Description = "Injection of an instance of a nullable abstract class type. If the container knows of an implementation for this abstract class type, it resolves to an instance of this type.")]
    AbstractionsNullableAbstractClassNotNullCase,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Generics, 
        Title = "Generic Implementation",
        Description = "A generic implementation type is resolvable.")]
    GenericsImplementation,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Generics, 
        Title = "Generic Abstraction",
        Description = "A generic abstraction type is resolvable.")]
    GenericsAbstraction,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Generics, 
        Title = "Open Generic Parameter Fill-Ins",
        Description = "If an abstraction has less generic parameter than its implementation, then upon resolution of the abstraction one or more generic parameters will inevitably remain open. The container is able to configure the fill-ins for these open generic parameters.")]
    GenericsOpenFillIns,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: IEnumerable<T>",
        Description = "Resolution of an IEnumerable<T>. It should iterate over an instance of each known implementation type of T.")]
    IterablesIEnumerable,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: Generic Array",
        Description = "Resolution of an array of T. It should contain an instance of each known implementation type of T.")]
    IterablesArray,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: IReadOnlyList<T>",
        Description = "Resolution of IReadOnlyList<T>. It should contain an instance of each known implementation type of T.")]
    IterablesIReadOnlyList,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: IList<T>",
        Description = "Resolution of IList<T>. It should contain an instance of each known implementation type of T.")]
    IterablesIList,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: ReadOnlyCollection<T>",
        Description = "Resolution of ReadOnlyCollection<T>. It should contain an instance of each known implementation type of T.")]
    IterablesReadOnlyCollection,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: List<T>",
        Description = "Resolution of List<T>. It should contain an instance of each known implementation type of T.")]
    IterablesList,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: Other Iterable Types",
        Description = "Resolution of remaining supported collection types. Each of the collection instance should contain an instance of each known implementation type of T.")]
    IterablesRemainingIterableTypes,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Iterables, 
        Title = "Iterable: Implementations Choice",
        Description = "With this feature an explicit set of implementations is configurable for collection injection. That way the amount of injected implementations can be limited, even if the container actually knows more.")]
    IterablesImplementationsChoice,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Factories, 
        Title = "Factory: Func<T>",
        Description = "Resolution of a Func<T>. Such a factory delays the resolution of a T-instance to whenever the owning component see fit. Also it allows to resolve multiple instances of T (as long as T isn't shared in the current scope).")]
    FactoriesFunc,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Factories, 
        Title = "Factory: Func<TA, T>",
        Description = "Resolution of a Func<TA, T> with TA-parameter being used for TA-dependencies in T. Works much like a resolution of a Func<T>. The difference is that the factory is able to resolve T with the help of a parameter of type TA. This is practical for parameter types which are usually not registered in the container (such as strings, enums or numeric types).")]
    FactoriesFuncWithParameter,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Factories, 
        Title = "Factory: Func<TA, T> (Sub-Dependencies)",
        Description = "Resolution of a Func<TA, T> with TA being used for TA-dependencies in T and in T's sub-dependencies transitively. Works like the ordinary Func<TA, T> resolution, but with TA also being used for sub-dependencies.")]
    FactoriesFuncWithParameterForSubDependencies,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Factories, 
        Title = "Factory: Lazy<T>",
        Description = "Resolution of a Lazy<T>. Such a factory (via the 'Value'-property of Lazy<T>) delays the resolution of a T-instance to whenever the owning component see fit. The conceptual difference to Func is that it can only resolve a single instance.")]
    FactoriesLazy,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Factories, 
        Title = "Factory: ThreadLocal<T>",
        Description = "Resolution of a ThreadLocal<T>. Such a factory (via the 'Value'-property of ThreadLocal<T>) delays the resolution of a T-instance to whenever the owning component see fit. It also creates a separate instance per thread.")]
    FactoriesThreadLocal,

    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Tuples, 
        Title = "Tuple: Tuple<T>",
        Description = "Resolution of Tuple. The tuple items should be resolved if they would be a resolution on their own.")]
    TuplesTuple,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Tuples, 
        Title = "Tuple: ValueTuple<T> (Syntax)",
        Description = "Resolution of the Syntax ValueTuple<T>. The tuple items should be resolved if they would be a resolution on their own.")]
    TuplesValueTupleSyntax,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Tuples, 
        Title = "Tuple: ValueTuple<T>",
        Description = "Resolution of ValueTuple<T>. The tuple items should be resolved if they would be a resolution on their own.")]
    TuplesValueTuple,
    
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.TypeInitializers, 
        Title = "Sync Type Initializer",
        Description = "There is an option to declare a synchronous (void) method as type initializer per type.")]
    TypeInitializersSync,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.TypeInitializers, 
        Title = "Async Type Initializer (Task)",
        Description = "There is an option to declare a asynchronous (Task) method as type initializer per type.")]
    TypeInitializersAsyncTask,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.TypeInitializers, 
        Title = "Async Type Initializer (ValueTask)",
        Description = "There is an option to declare a asynchronous (ValueTask) method as type initializer per type.")]
    TypeInitializersAsyncValueTask,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Injections, 
        Title = "Constructor Parameter Injection",
        Description = "Injection of a dependency into a constructor parameter.")]
    InjectionsConstructorParameter,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Injections, 
        Title = "Init Properties Implicit Injection",
        Description = "With this feature properties which have an init-accessor will be selected for property injection implicitly per default. Init-only properties can only be set during instantiation, so if the container resolves this type, then it makes sense that it automatically injects these properties.")]
    InjectionsInitPropertyImplicit,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Injections, 
        Title = "Explicit Properties Choice",
        Description = "A set of properties (with either init- or set-accessor) is configurable for property injection.")]
    InjectionsExplicitPropertyChoice,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Injections, 
        Title = "Type Initializer Parameter Injection",
        Description = "Injection of a dependency into a type initializer parameter.")]
    InjectionsTypeInitializerParameter,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "One Key: Single Item ([Key] T)",
        Description = "Injections of a single items for a single key.")]
    KeyedInjectionsOneKeySingleItem,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "One Key: Multiple Items ([Key] IReadOnlyList<T>)",
        Description = "Injections of a multiple items for a single key.")]
    KeyedInjectionsOneKeyMultipleItems,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "All Keys: Single Item (IReadOnlyDictionary<Key, T>)",
        Description = "Injections of a single items for all keys.")]
    KeyedInjectionsAllKeysSingleItem,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "All Keys: Multiple Items (IReadOnlyDictionary<Key, IReadOnlyList<T>>)",
        Description = "Injections of a multiple items for all keys.")]
    KeyedInjectionsAllKeysMultipleItems,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "Supported Key Types",
        Description = "Showcase of supported key types.")]
    KeyedInjectionsSupportedKeyTypes,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "Supported Map Types",
        Description = "Showcase of supported map types (dictionaries or iterables of KeyValuePair<Key, T>).")]
    KeyedInjectionsSupportedMapTypes,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.KeyedInjections, 
        Title = "Key Value Injection",
        Description = "Keyed implementation types can get their key value injected.")]
    KeyedInjectionsKeyValueInjection,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Async, 
        Title = "Injecting Async Dependency (Not Wrapped)",
        Description = "Injection of an async dependency (e.g. one which has an async type initializer), where the dependency isn't wrapped into a Task<T>, a ValueTask<T> or an IAsyncEnumerable<T>.")]
    AsyncNotWrapped,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Async, 
        Title = "Injecting Async Dependency (Task<T>)",
        Description = "Injection of an async dependency (e.g. one which has an async type initializer), where the dependency is wrapped into a Task<T>.")]
    AsyncWrappedTask,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Async, 
        Title = "Injecting Async Dependency (ValueTask<T>)",
        Description = "Injection of an async dependency (e.g. one which has an async type initializer), where the dependency is wrapped into a ValueTask<T>.")]
    AsyncWrappedValueTask,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Async, 
        Title = "Injecting Async Dependency (IEnumerable<T>)",
        Description = "Injection of an iterable of async dependencies (e.g. one which has an async type initializer), where the iterable of dependencies is not wrapped into an IAsyncEnumerable<T>.")]
    AsyncNotWrappedIEnumerable,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Async, 
        Title = "Injecting Async Dependencies (IAsyncEnumerable<T>)",
        Description = "Injection of an iterable of async dependencies (e.g. one which has an async type initializer), where the iterable of the dependencies is wrapped into an IAsyncEnumerable<T>.")]
    AsyncWrappedIAsyncEnumerable,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Scopes, 
        Title = "Creation of a Simple Scope",
        Description = "This container can create a simple scope. Simple means that the scope doesn't have a self-determined lifetime. It will be disposed when its parent scope is disposed.")]
    ScopesSimple,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Scopes, 
        Title = "Creation of a Transient Scope",
        Description = "This container can create a transient scope. Transient means that it can be disposed independently of its parent scope.")]
    ScopesTransient,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Scopes, 
        Title = "Configuration of Root Types",
        Description = "Dependency Types can be declared root types for scopes. That means, whenever the root type gets injected, it starts a new scope around it.")]
    ScopesRootTypes,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Scopes, 
        Title = "Reconfiguration of a Scope",
        Description = "Scopes can be reconfigured to a certain degree.")]
    ScopesReconfiguration,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Scopes, 
        Title = "Fragmentation by Root Types",
        Description = "Ability to use different scopes for different scope root types.")]
    ScopesFragmentationByRootTypes,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Scopes, 
        Title = "Default Fragment",
        Description = "If a scope root type has no specific scope fragment, then there is the option fallback to a configurable default scope (fragment).")]
    ScopesDefaultFragment,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.ScopedInstances, 
        Title = "Scoped Instances (Scope)",
        Description = "With this feature a type should be configurable to be instantiated once per scope and shared for all injections.")]
    ScopedInstancesScope,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.ScopedInstances, 
        Title = "Scoped Instances (Container)",
        Description = "With this feature a type should be configurable to be instantiated once for the whole container and shared for all injections. This is also commonly known as singleton or 'single instance'.")]
    ScopedInstancesContainer,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Disposal, 
        Title = "IDisposable",
        Description = "The container can manage the disposal of dependencies implementing IDisposable.")]
    DisposalIDisposable,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Disposal, 
        Title = "IAsyncDisposable",
        Description = "The container can manage the disposal of dependencies implementing IAsyncDisposable.")]
    DisposalIAsyncDisposable,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Disposal, 
        Title = "Transient IDisposable",
        Description = "The container can hand over responsibility to manage the disposal of dependencies implementing IDisposable to the user.")]
    DisposalTransient,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Disposal, 
        Title = "Transient IAsyncDisposal",
        Description = "The container can hand over responsibility to manage the disposal of dependencies implementing IAsyncDisposable to the user.")]
    DisposalAsyncTransient,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Disposal, 
        Title = "Scope Disposal",
        Description = "Transient scopes can be disposed eagerly (before the parent container is disposed).")]
    DisposalScope,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Disposal, 
        Title = "Add For Disposal",
        Description = "The container (or the scope) offers a way to add externally created instances to the disposal. That can become handy for disposable instances which are created via custom factories.")]
    DisposalAddForDisposal,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Customization, 
        Title = "Custom Factory",
        Description = "As part of the configuration of the container and/or the scopes the user has the option to embed own custom factory function which will be used for the returned type.")]
    CustomizationCustomFactory,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Customization, 
        Title = "Custom Constructor Parameter",
        Description = "Configurable customization of injection of specific constructor parameters.")]
    CustomizationCustomConstructorParameterInjection,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Customization, 
        Title = "Custom Property",
        Description = "Configurable customization of injection of specific properties.")]
    CustomizationCustomPropertyInjection,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Customization, 
        Title = "Custom Type Initializer Parameter",
        Description = "Configurable customization of injection of specific type initializer parameters.")]
    CustomizationCustomTypeInitializerInjection,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.DecoratorPattern, 
        Title = "Single Decorator",
        Description = "A single Decorator can be specified for an interface type.")]
    DecoratorPatternSingleDecorator,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.DecoratorPattern, 
        Title = "Multiple Decorator",
        Description = "Multiple Decorators can be specified for an interface type.")]
    DecoratorPatternMultipleDecorators,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.DecoratorPattern, 
        Title = "Explicit Order",
        Description = "If several Decorators are configured for an interface, then an explicit order can be configured. That way the user can make sure that one Decoration logic is executed before another.")]
    DecoratorPatternExplicitOrder,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.DecoratorPattern, 
        Title = "Different Order per Type",
        Description = "If several Decorators and implementation types are configured for an interface, then an explicit order can be configured per implementation type.")]
    DecoratorPatternDifferentOrderPerType,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.DecoratorPattern, 
        Title = "Default Decorator Order",
        Description = "If several Decorators are configured for an interface, then a default order can be configured. That order will be applied to implementation types which don't have an explicit order configured.")]
    DecoratorPatternDefaultOrder,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.CompositePattern, 
        Title = "Iterable Injection for Composites",
        Description = "Composite can get an Iterable of its interface injected in order to get the interface instances that it manages.")]
    CompositePatternIEnumerable,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.CompositePattern, 
        Title = "Composite Can Be Decorated",
        Description = "Composite can be decorated as well.")]
    CompositePatternDecorated,
    
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Misc, 
        Title = "Initialized Instances",
        Description = "Initialized Instances can be defined per container and/or scope and will be created automatically when its container/scope is created.")]
    MiscInitializedInstance,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Misc, 
        Title = "Marker Interfaces",
        Description = "Marker interfaces can be declared to mark certain dependency injection properties to implementation types (such as lifetime, disposal behavior and so on).")]
    MiscMarkerInterface,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Misc, 
        Title = "Register All Implementations",
        Description = "Register all implementation types of the container-hosting assembly and all referenced assemblies with a single configuration.")]
    MiscRegisterAllImplementations,
    [FeatureEnumInfo(
        FeatureGroup = FeatureGroup.Misc, 
        Title = "Register All Implementations of a Specific Assembly",
        Description = "Register all implementation types of a specified assembly with a single configuration.")]
    MiscRegisterAssemblyImplementations,
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
    // Group categories of resolvable types
    [EnumMember(Value = "Group of features concerned with resolving implementation types (non-abstract classes & structs).")]
    Implementations,
    [EnumMember(Value = "Group of features concerned with resolving abstraction types (interfaces & abstract classes).")]
    Abstractions,
    [EnumMember(Value = "Group of features concerned with resolving generic types.")]
    Generics,
    [EnumMember(Value = "Group of features concerned with resolving iterable types.")]
    Iterables,
    [EnumMember(Value = "Group of features concerned with resolving factory types. Factories here are functors (Func<T>, Lazy<T>) that are automatically generated by the DI container.")]
    Factories,
    [EnumMember(Value = "Group of features concerned with tuple types.")]
    Tuples,
    
    [EnumMember(Value = "Group of features concerned with type initializers. These are initialization methods that are called after the instance is created and before it is returned from the DI container. They are optionally declarable once per type.")]
    TypeInitializers,
    [EnumMember(Value = "Group of features concerned with injections.")]
    Injections,
    [EnumMember(Value = "Group of features concerned with keyed injections.")]
    KeyedInjections,
    
    [EnumMember(Value = "Group of features concerned with support for asynchronous programming.")]
    Async,
    
    [EnumMember(Value = "Group of features concerned with scopes.")]
    Scopes,
    [EnumMember(Value = "Group of features concerned with scoped instances. That means instances which are guaranteed to be created only once per configured scope. 'Scope' includes the container itself as the biggest possible scope as well here.")]
    ScopedInstances,
    [EnumMember(Value = "Group of features concerned with disposal of containers & other scopes and disposable resolved dependencies.")]
    Disposal,
    [EnumMember(Value = "Group of features concerned with customization of containers & other scopes.")]
    Customization,
    [EnumMember(Value = "Group of features concerned with the Decorator pattern.")]
    DecoratorPattern,
    [EnumMember(Value = "Group of features concerned with the Composite pattern.")]
    CompositePattern,
    [EnumMember(Value = "Group of features of miscellaneous nature.")]
    Misc,
    [EnumMember(Value = "Group of features that don't have an specific feature group yet.")]
    Other,
}