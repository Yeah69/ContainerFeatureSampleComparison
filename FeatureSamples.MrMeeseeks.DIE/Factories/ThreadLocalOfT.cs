using ContainerFeatureSampleComparison.FeatureDefinitions;
using MrMeeseeks.DIE.Configuration.Attributes;

[assembly:FeatureSample(Feature.FactoriesThreadLocal)]

namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE.Factories.ThreadLocalOfT;

internal class ConcreteClass
{
}

[ImplementationAggregation(typeof(ConcreteClass))]
// Instead of returning the implementation type directly, return a ThreadLocal<ConcreteClass>
// which is kind of a factory considering that the Value property creates an instance of the implementation type
[CreateFunction(typeof(ThreadLocal<ConcreteClass>), "Create")]
internal partial class Container
{
    private Container() {}
}

internal static class Usage
{
    internal static void Use()
    {
        using var container = Container.DIE_CreateContainer();
        var concreteClassFactory = container.Create();
            
        // Action that prints out ThreadName for the current thread
        var action = () =>
        {
            // If ThreadName.IsValueCreated is true, it means that we are not the first action to run on this thread.
            if (concreteClassFactory.IsValueCreated)
            {
                Console.WriteLine("Skipped");
            }
            else
            {
                // ThreadLocal objects defer the time of creation of the instance to when the Value property is called
                // Contrary to Func-factories, ThreadLocal-factories only create one instance per thread
                var value = concreteClassFactory.Value;
                Console.WriteLine("ThreadName = {0} {1}", Environment.CurrentManagedThreadId, value?.GetType().Name);
            }
        };

        // Launch eight times in parallel
        Parallel.Invoke(action, action, action, action, action, action, action, action);
    }
}