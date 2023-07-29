namespace ContainerFeatureSampleComparison.FeatureSamples.MrMeeseeks.DIE;

public class Program
{
    public static async Task Main()
    {
        await Disposal.Scope.Usage.Use();
    }
}