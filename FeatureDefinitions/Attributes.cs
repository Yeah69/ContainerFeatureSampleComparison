namespace ContainerFeatureSampleComparison.FeatureDefinitions;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class FeatureSampleAttribute : Attribute
{
    public FeatureSampleAttribute(Feature feature) { }
}

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class MissingFeatureAttribute : Attribute
{
    public MissingFeatureAttribute(Feature feature, MissingFeatureReason reason) { }
    public MissingFeatureAttribute(Feature feature, MissingFeatureReason reason, string hint) { }
}

[AttributeUsage(AttributeTargets.Assembly)]
public class DiContainerAttribute : Attribute
{
    public DiContainerAttribute(string name) { }
}

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class MiscellaneousInformationAttribute : Attribute
{
    public MiscellaneousInformationAttribute(MiscellaneousInformation information, string value) { }
}

[AttributeUsage(AttributeTargets.Assembly)]
public class ResolutionStageAttribute : Attribute
{
    public ResolutionStageAttribute(ResolutionStage stage) { }
}

[AttributeUsage(AttributeTargets.Field)]
public class FeatureGroupAssignmentAttribute : Attribute
{
    public FeatureGroupAssignmentAttribute(FeatureGroup group) { }
}

[AttributeUsage(AttributeTargets.Field)]
public class FeatureEnumInfoAttribute : Attribute
{
    public FeatureGroup FeatureGroup { get; set; } = (FeatureGroup)int.MaxValue;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = "No description provided.";
}
