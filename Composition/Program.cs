﻿// See https://aka.ms/new-console-template for more information

using System.Text;
using ContainerFeatureSampleComparison.Composition;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using Humanizer;

var compositionData = CompositionData.Create();

var html = new StringBuilder();
html.AppendLine("""
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Dependency Injection Container Feature Comparison</title>
        <meta charset="utf-8">
        <style>
            /* Material Design Styles */

            body {
              font-family: Roboto, Arial, sans-serif;
              margin: 0;
              padding: 0;
            }

            main {
              margin: 16px;
            }

            h1 {
              font-size: 32px;
              margin: 0;
              padding: 16px 0;
              color: #333;
            }

            h2 {
              font-size: 24px;
              margin: 0;
              padding: 8px 0;
              color: #333;
            }

            p {
              margin: 0;
              padding: 8px 0;
              color: #666;
            }

            table {
              border-collapse: collapse;
              width: 100%;
              margin-top: 16px;
            }

            th {
              text-align: left;
              padding: 8px;
              color: #333;
            }

            td.header {
              font-weight: bold;   
              padding: 8px;
              color: #333;
            }

            td {
              padding: 8px;
              color: #666;
            }

            tbody tr:nth-child(even) {
              background-color: #f9f9f9;
            }

            pre {
              margin: 0;
              padding: 8px;
              background-color: #f9f9f9;
              color: #666;
            }
            
            button {
              padding: 8px;
              height: 100%;
              width: 100%;
              color: #fff;
              text-align: left;
              border: 0;
              cursor: default;
            }
            
            button.zoom {
              cursor: zoom-in;
            }
            
            /* Background colors picked from Okabe-Ito (should be color-blind-friendly; https://jfly.uni-koeln.de/color/). Hex values from https://mikemol.github.io/technique/colorblind/2018/02/11/color-safe-palette.html. */

            .status_check {
              color: white;
              background-color: #009E73;
              border: 3px solid black;
            }

            .status_warn {
              color: white;
              background-color: #0072B2;
              border: 3px dashed black;
            }

            .status_cross {
              color: white;
              background-color: #D55E00;
              border: 3px dotted black;
            }
    </style>
    </head>
    <body>
        <main>
            <div>
                <h1>Dependency Injection Container Feature Comparison</h1>
""");

html.AppendLine($$"""
<section>
    <table>
        <tbody>
            <tr>
                <td colspan="{{compositionData.DiContainerNames.Count + 1}}">
                    <h2>General Information</h2>
                </td>
            </tr>
""");
    
html.AppendLine("""
            <tr>
                <td></td>
""");

foreach (var diContainerName in compositionData.DiContainerNames)
{
    html.AppendLine($$"""
                <td class="header">{{diContainerName.EscapeHtmlCharacters()}}</td>
""");
}

html.AppendLine("""
            </tr>
""");

foreach (var miscellaneousInformation in Enum.GetValues(typeof(MiscellaneousInformation)).OfType<MiscellaneousInformation>())
{
    html.AppendLine($$"""
            <tr>
                <td>{{miscellaneousInformation.Humanize(LetterCasing.Title).EscapeHtmlCharacters()}}</td>
""");

    foreach (var diContainerName in compositionData.DiContainerNames)
    {
        if (compositionData.AllAvailableMiscellaneousInformation.TryGetValue((diContainerName, miscellaneousInformation),
                out var information))
        {
            if (Uri.TryCreate(information, UriKind.Absolute, out var uri))
            {
                html.AppendLine($$"""
<td><a href="{{uri}}">Link</a></td>
""");
            }
            else
            {
                html.AppendLine($$"""
<td>{{information.EscapeHtmlCharacters()}}</td>
""");
            }
        }
        else
        {
            html.AppendLine("""
<td>🤷 Unknown</td>
""");
        }
    }

    html.AppendLine("""
            </tr>
""");
}

html.AppendLine("""
            <tr>
                <td>Resolution Stage</td>
""");

foreach (var diContainerName in compositionData.DiContainerNames)
{
    if (compositionData.AllResolutionStage.TryGetValue(diContainerName, out var resolutionStage))
    {
        var text = resolutionStage is ResolutionStage.CompileTime
            ? "🔨 Compile-Time"
            : resolutionStage is ResolutionStage.RunTime
                ? "🏃 Run-Time"
                : "🤷 Unknown";
        html.AppendLine($$"""
<td>{{text.EscapeHtmlCharacters()}}</td>
""");
    }
    else
    {
        html.AppendLine("""
<td>🤷 Unknown</td>
""");
    }
}

html.AppendLine("""
            </tr>
""");

foreach (var featureGroupDescription in compositionData.FeatureGroupDescriptions.OrderBy(g => g.Features.MinBy(f => f.Feature)?.Feature ?? (Feature) int.MaxValue))
{
    html.AppendLine($$"""
            <tr>
                <td colspan="{{compositionData.DiContainerNames.Count + 1}}">
                    <h2>{{featureGroupDescription.Title.Humanize(LetterCasing.Title).EscapeHtmlCharacters()}}</h2>
                    <div>{{(!string.IsNullOrEmpty(featureGroupDescription.Description) ? $"<p>{featureGroupDescription.Description.EscapeHtmlCharacters()}</p>" : string.Empty)}}</div>
                </td>
            </tr>
""");
    
    html.AppendLine("""
<tr>
    <td class="header">Feature</td>
""");

    foreach (var diContainerName in compositionData.DiContainerNames)
    {
        html.AppendLine($$"""
<td class="header">{{diContainerName.EscapeHtmlCharacters()}}</td>
""");
    }

    html.AppendLine("""
</tr>
""");
    
    foreach (var featureDescription in featureGroupDescription.Features.OrderBy(f => f.Feature))
    {
        var title = featureDescription.Title.Contains(' ')
            ? featureDescription.Title
            : featureDescription.Title.Humanize(LetterCasing.Title);
        html.AppendLine($$"""
<tr>
    <td><div class="zoom" onclick="openDescriptionBox('{{compositionData.IdMap[featureDescription]}}')">{{title.EscapeHtmlCharacters()}} 🔍</div></td>
""");

        foreach (var diContainerName in compositionData.DiContainerNames)
        {
            if (compositionData.AllAvailableFeatureDescriptions.TryGetValue(
                    (diContainerName, featureDescription.Feature), 
                    out var specificFeatureDescription))
            {
                var text = specificFeatureDescription switch
                {
                    FeatureSampleDescription featureSampleDescription => GenerateFeatureStateCell("✓ Sample", "status_check",featureSampleDescription),
                    MissingFeatureDescription { Hint: null } missingFeatureDescription => missingFeatureDescription.Reason switch
                    {
                        MissingFeatureReason.Unimplemented => GenerateFeatureStateCell("⚠ Unimplemented", "status_warn"),
                        MissingFeatureReason.NotSupported => GenerateFeatureStateCell("⚠ Not supported", "status_warn"),
                        MissingFeatureReason.DesignDecision => GenerateFeatureStateCell("⚠ Design decision", "status_warn"),
                        _ => throw new ArgumentOutOfRangeException(nameof(missingFeatureDescription.Reason))
                    },
                    MissingFeatureDescription { Hint: not null } missingFeatureDescription => missingFeatureDescription.Reason switch
                    {
                        MissingFeatureReason.Unimplemented => GenerateFeatureStateCell("⚠ Unimplemented", "status_warn", missingFeatureDescription),
                        MissingFeatureReason.NotSupported => GenerateFeatureStateCell("⚠ Not supported", "status_warn", missingFeatureDescription),
                        MissingFeatureReason.DesignDecision => GenerateFeatureStateCell("⚠ Design decision", "status_warn", missingFeatureDescription),
                        _ => throw new ArgumentOutOfRangeException(nameof(missingFeatureDescription.Reason))
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(specificFeatureDescription))
                };
                html.AppendLine($$"""
<td>{{text}}</td>
""");
            }
            else
            {
                html.AppendLine($"<td>{GenerateFeatureStateCell("🤷 Unknown", "status_cross")}</td>");
            }

            string GenerateFeatureStateCell(string label, string styleClass, IFeatureDescription? associatedFeatureDescription = null)
            {
                var onclick = associatedFeatureDescription is not null
                    ? $" onclick=\"openDescriptionBox('{compositionData.IdMap[associatedFeatureDescription]}')\""
                    : "";
                var labelSuffix = associatedFeatureDescription is not null
                    ? " 🔍"
                    : "";
                var cursorClass = associatedFeatureDescription is not null
                    ? " zoom"
                    : "";
                return $"<button class=\"{styleClass}{cursorClass}\"{onclick}>{label.EscapeHtmlCharacters()}{labelSuffix.EscapeHtmlCharacters()}</button>";
            }
        }

        html.AppendLine("""
</tr>
""");
            
        html.AppendLine($$"""
<tr id="{{compositionData.IdMap[featureDescription]}}" class="description_box">
    <td colspan="{{compositionData.DiContainerNames.Count + 1}}">
        <pre>{{featureDescription.Description.EscapeHtmlCharacters()}}</pre>
    </td>
</tr>
""");

        foreach (var diContainerName in compositionData.DiContainerNames)
        {
            if (compositionData.AllAvailableFeatureDescriptions.TryGetValue(
                    (diContainerName, featureDescription.Feature), 
                    out var specificFeatureDescription))
            {
                if (specificFeatureDescription is FeatureSampleDescription featureSampleDescription)
                {
                    html.AppendLine($$"""
<tr id="{{compositionData.IdMap[featureSampleDescription]}}" class="description_box">
    <td colspan="{{compositionData.DiContainerNames.Count + 1}}">
        <pre><code class="hljs language-csharp">{{featureSampleDescription.SampleCode.EscapeHtmlCharacters()}}</code></pre>
    </td>
</tr>
""");
                }
                if (specificFeatureDescription is MissingFeatureDescription { Hint: {} hint } missingFeatureDescription)
                {
                    html.AppendLine($$"""
<tr id="{{compositionData.IdMap[missingFeatureDescription]}}" class="description_box">
    <td colspan="{{compositionData.DiContainerNames.Count + 1}}">
        <pre>{{hint.EscapeHtmlCharacters()}}</pre>
    </td>
</tr>
""");
                }
            }
        }
    }
}
    
html.AppendLine("""
        </tbody>
    </table>
</section>
""");

html.AppendLine("""
            </div>
        </main>
<!-- Shout out to highlight.js project: https://github.com/highlightjs/highlight.js -->
<link rel="stylesheet" href="styles/codepen-embed.min.css">
<script src="scripts/highlight.min.js"></script>
        <script>
hljs.highlightAll();
const description_boxes = document.querySelectorAll('.description_box');
closeAllDescriptionBoxes();
function closeAllDescriptionBoxes() {
    for (const description_box of description_boxes) {
        description_box.style.display = 'none';
    }
}
function openDescriptionBox(id) {
    closeAllDescriptionBoxes();
    const description_box = document.getElementById(id);
    description_box.style.display = 'table-row';
}
        </script>

    </body>
</html>
""");

Directory.CreateDirectory("./wwwroot");
File.WriteAllText("./wwwroot/index.html", html.ToString());

Console.WriteLine("Hello, World!");