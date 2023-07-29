namespace ContainerFeatureSampleComparison.FeatureSamples.Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static void Main()
    {
        ScopedInstances.Container.Usage.Use();
    }
}