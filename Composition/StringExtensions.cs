namespace ContainerFeatureSampleComparison.Composition;

internal static class StringExtensions
{
    internal static string EscapeHtmlCharacters(this string str) => str
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;")
        .Replace("\"", "&quot;")
        .Replace("'", "&#39;");
}